using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public static Checker instance;

    private Vector2Int[] _directions = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    [SerializeField]
    private GameObject[][] _dots;
    [SerializeField]
    private Transform _dotsContainer;
    [SerializeField]
    private List<GameObject> _dotsAvailable;

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        _dotsAvailable = new List<GameObject>();
    }

    public void AddAvailableDots()
    {
        foreach (Transform dot in _dotsContainer)
        {
            _dotsAvailable.Add(dot.gameObject);
        }
    }

    public void RemoveAvailableDots(GameObject dot)
    {
        _dotsAvailable.Remove(dot);
        if (_dotsAvailable.Count == 0)
        {
            GameManager.instance.FinalizeGame();
        }
    }

    public GameObject[] GetAvailableDots()
    {
        return _dotsAvailable.ToArray();
    }

    public GameObject[] GetDotsSequenceAI(GameObject dotToCheck)
    {
        List<GameObject> dots = new List<GameObject>();

        Vector2Int dir = Vector2Int.down;

        Vector2Int[] sortedDirections = GetSortedDirections(dir);
        Vector3Int currentCellPos = GridCellManager.instance.GetCellOfWorldPosition(dotToCheck.transform.position);
        GameObject currentDot = dotToCheck;
        GameObject nextDot = dotToCheck;

        Vector2Int direction = sortedDirections[0];
        int dotsCount = 0;
        dots.Add(dotToCheck);

        for (int i = 0; i < sortedDirections.Length - 1; i++)
        {
            direction = sortedDirections[i];
            currentCellPos += (Vector3Int)direction;

            currentDot = nextDot;
            nextDot = DotInitializer.instance.GetDot(currentCellPos);

            if (nextDot == null) break;
            if (!currentDot.GetComponent<Dot>().IsLinkedDot(nextDot) || !nextDot.GetComponent<Dot>().IsLinkedDot(currentDot)) break;

            dots.Add(nextDot);
            dotsCount++;
        }

        dir = Vector2Int.right;
        sortedDirections = GetSortedDirections(dir);
        currentCellPos = GridCellManager.instance.GetCellOfWorldPosition(dotToCheck.transform.position);    
        currentDot = dotToCheck;
        nextDot = dotToCheck;
        direction = sortedDirections[0];
        for (int i = 0; i < sortedDirections.Length - 1; i++)
        {
            direction = sortedDirections[i];
            currentCellPos += (Vector3Int)direction;

            currentDot = nextDot;
            nextDot = DotInitializer.instance.GetDot(currentCellPos);

            if (nextDot == null) break;
            if (!currentDot.GetComponent<Dot>().IsLinkedDot(nextDot) || !nextDot.GetComponent<Dot>().IsLinkedDot(currentDot)) break;

            dots.Insert(0,nextDot);
            dotsCount++;
        }
        

        if (dotsCount == 3)
        {
            if (dots[0].GetComponent<Dot>().IsLinkedDot(dots[3]) || dots[3].GetComponent<Dot>().IsLinkedDot(dots[0]))
            {
                return null;
            }
            return
                    new GameObject[]
                    {
                        dots[3],
                        dots[0],
                    };
        }
        else
        {
            return null;
        }
    }


    public void CheckAroundDot(GameObject dotToCheck)
    {
        _dots = new GameObject[4][];
        for (int i = 0; i < _directions.Length; i++)
        {
            _dots[i] = GetDotsSequence(dotToCheck, _directions[i]);
        }

        foreach (var dot in _dots)
        {
            if (dot == null) continue;
            if (dot.Length == 4)
            {
                Vector3 center = GetDotsCenter(dot);
                PointPlacer.instance.PlacePoint(center);
            }
        }
    }

    public Vector3 GetDotsCenter(GameObject[] dots)
    {
        Vector3 center = Vector3.zero;
        foreach (var dot in dots)
        {
            center += dot.transform.position;
        }
        center /= dots.Length;
        return center;
    }

    private GameObject[] GetDotsSequence(GameObject dotToCheck, Vector2Int dir)
    {
        GameObject[] dots = new GameObject[4];
        dots[0] = dotToCheck;

        Vector2Int[] sortedDirections = GetSortedDirections(dir);
        Vector3Int currentCellPos = GridCellManager.instance.GetCellOfWorldPosition(dotToCheck.transform.position);
        GameObject currentDot = dotToCheck;
        GameObject nextDot = dotToCheck;

        for (int i = 0; i < sortedDirections.Length - 1; i++)
        {
            Vector2Int direction = sortedDirections[i];
            currentCellPos += (Vector3Int)direction;

            currentDot = nextDot;
            nextDot = DotInitializer.instance.GetDot(currentCellPos);

            if (nextDot == null) return null;
            if (!currentDot.GetComponent<Dot>().IsLinkedDot(nextDot) || !nextDot.GetComponent<Dot>().IsLinkedDot(currentDot))
            {
                return null;
            }

            dots[i + 1] = nextDot;
        }

        if (!dots[0].GetComponent<Dot>().IsLinkedDot(dots[3]) && !dots[3].GetComponent<Dot>().IsLinkedDot(dots[0]))
        {
            return null;
        }

        foreach (var dot in dots)
        {
            if (dot == null) return null;
        }

        return dots;
    }

    public Vector2Int[] GetSortedDirections(Vector2Int dir)
    {
        Vector2Int[] sortedDirections = new Vector2Int[4];
        for (int i = 0; i < _directions.Length; i++)
        {
            if (_directions[i] == dir)
            {
                sortedDirections[0] = _directions[i];
            }
            else if (_directions[i] == -dir)
            {
                sortedDirections[2] = _directions[i];
            }
            else
            {
                sortedDirections[1] = _directions[i];
            }
        }
        sortedDirections[3] = -sortedDirections[1];
        return sortedDirections;
    }


    //private void OnDrawGizmos()
    //{
    //    if (_dots != null)
    //    {
    //        for (int i = 0; i < _dots.Length; i++)
    //        {
    //            if (_dots[i] == null) continue;
    //            for (int j = 0; j < _dots[i].Length; j++)
    //            {
    //                if (i == 0)
    //                {
    //                    Gizmos.color = Color.green;
    //                }
    //                else if (i == 1)
    //                {
    //                    Gizmos.color = Color.blue;
    //                }
    //                else if (i == 2)
    //                {
    //                    Gizmos.color = Color.yellow;
    //                }
    //                else
    //                {
    //                    Gizmos.color = Color.magenta;
    //                }
    //                Gizmos.DrawSphere(_dots[i][j].transform.position, 0.1f);
    //            }
    //        }
    //    }
    //}
}

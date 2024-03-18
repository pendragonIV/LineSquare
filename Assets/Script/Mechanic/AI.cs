using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField]
    private GameObject[][] _dotsPairAvailable;

    private Vector2Int[] _directions = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left,
    };

    public void CalculateSolution()
    {
        if(GameManager.instance.IsGameFinal())
        {
            return;
        }
        if (GetDotMatchThreeAvailable())
        {
            return;
        }
        GameObject[] dots = Checker.instance.GetAvailableDots();
        int random = Random.Range(0, dots.Length);
        GameObject randomDot = dots[random];
        Vector3Int randomDotCell = GridCellManager.instance.GetCellOfWorldPosition(randomDot.transform.position);
        Vector3Int next = GetPlaceableNextDot(randomDotCell, randomDot);
        if (randomDot != null)
        {
            StartCoroutine(PlaceNewline(randomDotCell, next));
        }
    }

    private Vector3Int GetPlaceableNextDot(Vector3Int start, GameObject startGO)
    {
        Vector2Int[] directions = GetRandomSortedDirections();
        foreach(Vector2Int direction in directions)
        {
            Vector3Int next = start + (Vector3Int)direction;
            GameObject nextDot = DotInitializer.instance.GetDot(next);
            if (nextDot != null)
            {
                if (!nextDot.GetComponent<Dot>().IsLinkedDot(startGO) || !startGO.GetComponent<Dot>().IsLinkedDot(nextDot))
                {
                    return next;
                }
            }
        }
        return Vector3Int.zero;
    }

    public IEnumerator PlaceNewline(Vector3Int firstDotCell, Vector3Int secondDotCell)
    {
        yield return new WaitForSeconds(1.5f);
        while (!LineInitializer.instance.AILineInitializingManager(firstDotCell, secondDotCell))
        {
            GameObject randomDot = DotInitializer.instance.GetRandomDot();
            firstDotCell = GridCellManager.instance.GetCellOfWorldPosition(randomDot.transform.position);
            Vector2Int[] directions = GetRandomSortedDirections();
            foreach (Vector2Int direction in directions)
            {
                secondDotCell = firstDotCell + (Vector3Int)direction;
                if (LineInitializer.instance.AILineInitializingManager(firstDotCell, secondDotCell))
                {
                    break;
                }
            }
        }
    }


    private Vector2Int[] GetRandomSortedDirections()
    {
        Vector2Int[] directions = new Vector2Int[4];
        for(int i = 0; i < _directions.Length; i++)
        {
            directions[i] = _directions[i];
        }

        for(int i = 0; i < directions.Length; i++)
        {
            int random = Random.Range(0, directions.Length);
            Vector2Int temp = directions[i];
            directions[i] = directions[random];
            directions[random] = temp;
        }
        return directions;
    }

    private bool GetDotMatchThreeAvailable()
    {
        GameObject[] dots = Checker.instance.GetAvailableDots();
        foreach(GameObject dot in dots)
        {
            GameObject[] matchThree = Checker.instance.GetDotsSequenceAI(dot);
            if(matchThree != null)
            {
                Vector3Int firstDotCell = GridCellManager.instance.GetCellOfWorldPosition(matchThree[0].transform.position);
                Vector3Int secondDotCell = GridCellManager.instance.GetCellOfWorldPosition(matchThree[1].transform.position);
                Vector2Int matchDir = (Vector2Int)(firstDotCell - secondDotCell);
                if (matchDir == Vector2Int.zero) continue;
                StartCoroutine(PlaceNewlineForMatchSequence(firstDotCell, secondDotCell));
                return true;
            }
        }
        return false;
    }

    public IEnumerator PlaceNewlineForMatchSequence(Vector3Int firstDotCell, Vector3Int secondDotCell)
    {
        yield return new WaitForSeconds(1.5f);
        LineInitializer.instance.AILineInitializingManager(firstDotCell, secondDotCell);
    }
}

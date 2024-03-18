using UnityEngine;

public class Dot : MonoBehaviour
{
    private GameObject[] linkedDot;
    private int _linkedDotCount = 0;

    private void Start()
    {
        int maxLinkedDot = MaxLinkedDot();
        linkedDot = new GameObject[maxLinkedDot];
    }

    public void AddLinkedDot(GameObject dot)
    {
        for (int i = 0; i < linkedDot.Length; i++)
        {
            if (linkedDot[i] == null)
            {
                linkedDot[i] = dot;
                _linkedDotCount++;
                if (IsLinkedDotFull())
                {
                    Checker.instance.RemoveAvailableDots(gameObject);
                }
                return;
            }
        }
    }

    public int GetLinkedDotCount()
    {
        return _linkedDotCount;
    }

    public void RemoveLinkedDot(GameObject dot)
    {
        for (int i = 0; i < linkedDot.Length; i++)
        {
            if (linkedDot[i] == dot)
            {
                linkedDot[i] = null;
                return;
            }
        }
    }

    public bool IsLinkedDot(GameObject dot)
    {
        for (int i = 0; i < linkedDot.Length; i++)
        {
            if (linkedDot[i] == dot)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsLinkedDotFull()
    {
        for (int i = 0; i < linkedDot.Length; i++)
        {
            if (linkedDot[i] == null)
            {
                return false;
            }
        }
        return true;
    }

    private int MaxLinkedDot()
    {
        int maxLinkedDot = 0;

        Vector3Int cell = GridCellManager.instance.GetCellOfWorldPosition(transform.position);
        Vector3Int nextLeft = cell + Vector3Int.left;
        Vector3Int nextRight = cell + Vector3Int.right;
        Vector3Int nextUp = cell + Vector3Int.up;
        Vector3Int nextDown = cell + Vector3Int.down;

        Vector3Int[] nextCells = new Vector3Int[]
        {
            nextLeft,
            nextRight,
            nextUp,
            nextDown
        };

        foreach (var nextCell in nextCells)
        {
            GameObject nextDot = DotInitializer.instance.GetDot(nextCell);
            if (nextDot != null)
            {
                maxLinkedDot++;
            }
        }

        return maxLinkedDot;
    }
}

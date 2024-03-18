using UnityEngine;

public class LineInitializer : MonoBehaviour
{
    public static LineInitializer instance;

    [SerializeField]
    private GameObject _linePrefab;
    [SerializeField] 
    private GameObject _AILinePrefab;
    [SerializeField]
    private Transform _lineContainer;


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

    private void Update()
    {
        if (GameManager.instance.IsGameFinal() || !GameManager.instance.IsPlayerTurn())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            LineInitializingManager();
        }
    }

    public bool AILineInitializingManager(Vector3Int firstDotCell, Vector3Int secondDotCell)
    {
        GameObject currentDotGO = DotInitializer.instance.GetDot(firstDotCell);
        GameObject nextDotGO = DotInitializer.instance.GetDot(secondDotCell);

        if (currentDotGO == null || nextDotGO == null)
        {
            return false;
        }

        Dot currentDot = currentDotGO.GetComponent<Dot>();
        Dot nextDot = nextDotGO.GetComponent<Dot>();

        if (currentDot.IsLinkedDot(nextDotGO) || nextDot.IsLinkedDot(currentDotGO))
        {
            return false;
        }
        Vector3Int direction = secondDotCell - firstDotCell;
        InitNewLine(firstDotCell, secondDotCell, (Vector2Int)direction, GameManager.instance.IsPlayerTurn());

        currentDot.AddLinkedDot(nextDotGO);
        nextDot.AddLinkedDot(currentDotGO);

        Checker checker = Checker.instance;
        checker.CheckAroundDot(currentDotGO);
        checker.CheckAroundDot(nextDotGO);

        GameManager.instance.ChangeTurn();
        return true;
    }

    private void LineInitializingManager()
    {
        Vector3Int cellPos = GetClickingCell();
        Vector3 cellWordPosition = GridCellManager.instance.GetWordPositionOfCell(cellPos);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2Int direction = Vector2Int.FloorToInt(CalculateDirection(cellWordPosition, mousePos));
        Vector3Int nextCell = cellPos + new Vector3Int(direction.x, direction.y, 0);

        GameObject currentDotGO = DotInitializer.instance.GetDot(cellPos);
        GameObject nextDotGO = DotInitializer.instance.GetDot(nextCell);

        if (currentDotGO == null || nextDotGO == null)
        {
            return;
        }

        Dot currentDot = currentDotGO.GetComponent<Dot>();
        Dot nextDot = nextDotGO.GetComponent<Dot>();

        if (currentDot.IsLinkedDot(nextDotGO) || nextDot.IsLinkedDot(currentDotGO))
        {
            return;
        }
        InitNewLine(cellPos, nextCell, direction, GameManager.instance.IsPlayerTurn());

        currentDot.AddLinkedDot(nextDotGO);
        nextDot.AddLinkedDot(currentDotGO);

        Checker checker = Checker.instance;
        checker.CheckAroundDot(currentDotGO);
        checker.CheckAroundDot(nextDotGO);

        GameManager.instance.ChangeTurn();
    }

    private Vector3Int GetClickingCell()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = GridCellManager.instance.GetCellOfWorldPosition(mousePos);
        return cellPos;
    }

    private void InitNewLine(Vector3Int cellPos, Vector3Int nextCell, Vector2Int direction, bool isPlayerLine)
    {
        Vector3 cellWordPosition = GridCellManager.instance.GetWordPositionOfCell(cellPos);
        Vector3 nextCellWordPosition = GridCellManager.instance.GetWordPositionOfCell(nextCell);

        Vector3 linePosition = (cellWordPosition + nextCellWordPosition) / 2;

        float rotation = CalculateRotation(cellWordPosition, nextCellWordPosition);

        if (isPlayerLine)
        {
            GameObject line = Instantiate(_linePrefab, linePosition, Quaternion.Euler(0, 0, rotation), _lineContainer);
        }
        else
        {
            GameObject line = Instantiate(_AILinePrefab, linePosition, Quaternion.Euler(0, 0, rotation), _lineContainer);
        }
    }

    private Vector2 CalculateDirection(Vector2 startPos, Vector2 endPos)
    {
        if (Mathf.Abs(startPos.x - endPos.x) > Mathf.Abs(startPos.y - endPos.y))
        {
            if (startPos.x > endPos.x)
            {
                return Vector2.left;
            }
            else
            {
                return Vector2.right;
            }
        }
        else
        {
            if (startPos.y > endPos.y)
            {
                return Vector2.down;
            }
            else
            {
                return Vector2.up;
            }
        }
    }

    private float CalculateRotation(Vector2 startPos, Vector2 endPos)
    {
        Vector2 direction = CalculateDirection(startPos, endPos);
        if (direction == Vector2.left)
        {
            return 180;
        }
        else if (direction == Vector2.right)
        {
            return 0;
        }
        else if (direction == Vector2.up)
        {
            return 90;
        }
        else
        {
            return -90;
        }
    }
}

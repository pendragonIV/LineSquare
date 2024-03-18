using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap _tileMap;

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
    public void SetMapForThisManager(Tilemap tilemap)
    {
        if (tilemap != null)
        {
            this._tileMap = tilemap;
        }
    }

    public float GetCellSize()
    {
        return _tileMap.cellSize.x;
    }

    public void ReSetTilemapPosition(int height, int width)
    {
        float cellSize = _tileMap.cellSize.x;
        _tileMap.transform.position = new Vector3(-width * cellSize / 2, -height * cellSize / 2, 0);
    }


    public bool IsThisAreaCanPlace(Vector3Int cellPos)
    {
        return _tileMap.GetTile(cellPos) != null;
    }

    public Vector3Int GetCellOfWorldPosition(Vector3 position)
    {
        Vector3Int cellPosition = _tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 GetWordPositionOfCell(Vector3Int cellPosition)
    {
        return _tileMap.GetCellCenterWorld(cellPosition);
    }
}

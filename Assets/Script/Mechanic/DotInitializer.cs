using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DotInitializer : MonoBehaviour
{
    public static DotInitializer instance;

    [SerializeField]
    private GameObject _dotPrefab;
    [SerializeField]
    private Transform _dotContainer;
    [SerializeField]
    private Dictionary<Vector3Int, GameObject> _dotInitialized;

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
        _dotInitialized = new Dictionary<Vector3Int, GameObject>();
    }

    public void InitializingDotForGame(int width, int height)
    {
        Vector3Int cellStart = Vector3Int.zero;
        for (int i = 0; i < height; i++)
        {
            cellStart.y = i;
            for (int j = 0; j < width; j++)
            {
                cellStart.x = j;
                Vector3 worldPos = GridCellManager.instance.GetWordPositionOfCell(cellStart);
                GameObject dot = Instantiate(_dotPrefab, worldPos, Quaternion.identity, _dotContainer);
                _dotInitialized.Add(cellStart, dot);
                //dot.GetComponent<Dot>().SetCellPosition(cellStart);
            }
        }

        ReSetContainerPosition(height, width);
        GridCellManager.instance.ReSetTilemapPosition(height, width);
    }

    private void ReSetContainerPosition(int height, int width)
    {
        float cellSize = GridCellManager.instance.GetCellSize();
        _dotContainer.transform.position = new Vector3(-width * cellSize / 2, -height * cellSize / 2, 0);
    }

    public GameObject GetDot(Vector3Int cellPos)
    {
        if (_dotInitialized.ContainsKey(cellPos))
        {
            return _dotInitialized[cellPos];
        }
        return null;
    }

    public GameObject GetRandomDot()
    {
        int randomIndex = Random.Range(0, _dotInitialized.Count);
        GameObject randomDot = _dotInitialized.ElementAt(randomIndex).Value;
        while (randomDot.GetComponent<Dot>().IsLinkedDotFull())
        {
            randomIndex = Random.Range(0, _dotInitialized.Count);
            randomDot = _dotInitialized.ElementAt(randomIndex).Value;
        }
        return randomDot;
    }
}

using UnityEngine;

public class CenterCell : MonoBehaviour
{
    private void Start()
    {
        PutObjectToCenterCell();
    }
    public void PutObjectToCenterCell()
    {
        Vector3Int cellPos = GridCellManager.instance.GetCellOfWorldPosition(transform.position);
        this.transform.position = GridCellManager.instance.GetWordPositionOfCell(cellPos);
    }
}

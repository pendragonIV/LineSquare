using DG.Tweening;
using UnityEngine;

public class PointPlacer : MonoBehaviour
{
    public static PointPlacer instance;
    [SerializeField]
    private GameObject _pointPrefab;
    [SerializeField]
    private GameObject _AIPointPrefab;
    [SerializeField]
    private GameObject _pointContainer;
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
    public void PlacePoint(Vector3 position)
    {
        if (IsPointPlacedInThisPosition(position)) return;
        GameObject point;
        if (GameManager.instance.IsPlayerTurn())
        {
            point = Instantiate(_pointPrefab, position, Quaternion.identity, _pointContainer.transform);
        }
        else
        {
            point = Instantiate(_AIPointPrefab, position, Quaternion.identity, _pointContainer.transform);
        }
        GameManager.instance.AddScore();
        point.transform.localScale = Vector3.one * 2;
        point.transform.DOScale(Vector3.one, 0.4f);
        point.GetComponent<SpriteRenderer>().color = Color.clear;
        point.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.3f);
    }

    private bool IsPointPlacedInThisPosition(Vector3 position)
    {
        Collider2D check = Physics2D.OverlapPoint(position);
        if (check && check.CompareTag("Point"))
        {
            return true;
        }
        return false;
    }
}

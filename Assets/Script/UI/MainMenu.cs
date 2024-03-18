using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform _gameLogo;
    [SerializeField]
    private Transform _tutorPanel;
    [SerializeField]
    private Transform _guideLine;
    [SerializeField]
    private Transform _playBtn;
    [SerializeField]
    private Transform _sceneComponents;


    private void Start()
    {
        _tutorPanel.gameObject.SetActive(false);

        _gameLogo.GetComponent<CanvasGroup>().alpha = 0f;
        _gameLogo.GetComponent<CanvasGroup>().DOFade(1, 2f).SetUpdate(true);

        _gameLogo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 300, 0);
        _gameLogo.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 90), 2f, false).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void ShowTutorPanel()
    {
        _tutorPanel.gameObject.SetActive(true);
        _guideLine.gameObject.SetActive(true);
        FadeIn(_tutorPanel.GetComponent<CanvasGroup>(), _guideLine.GetComponent<RectTransform>());
        _sceneComponents.gameObject.SetActive(false);
        _playBtn.gameObject.SetActive(false);
    }

    public void HideTutorPanel()
    {
        StartCoroutine(FadeOut(_tutorPanel.GetComponent<CanvasGroup>(), _guideLine.GetComponent<RectTransform>()));
        _sceneComponents.gameObject.SetActive(true);
        _playBtn.gameObject.SetActive(true);
    }

    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(-1280, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutBack).SetUpdate(true);
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(1280, 0), .3f, false).SetEase(Ease.InOutBack).SetUpdate(true);

        yield return new WaitForSecondsRealtime(.3f);
        _guideLine.gameObject.SetActive(true);
        _tutorPanel.gameObject.SetActive(false);
    }

}

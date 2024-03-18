using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform _scoreBoard;

    [SerializeField]
    private Text _playerScore;
    [SerializeField]
    private Text _botScore;

    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private Sprite _player;
    [SerializeField]
    private Sprite _bot;

    private void Start()
    {
        _gameOverPanel.SetActive(false);
    }

    public void FinalizeGame(bool isPlayerWin)
    {
        _gameOverPanel.SetActive(true);
        _gameOverPanel.GetComponent<CanvasGroup>().alpha = 0f;
        _gameOverPanel.GetComponent<CanvasGroup>().DOFade(1f, .5f);
        if(isPlayerWin)
        {
            _gameOverPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = _player;
            _gameOverPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = _bot;
        }
        else
        {
            _gameOverPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = _bot;
            _gameOverPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = _player;
        }
        _gameOverPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().SetNativeSize();
        _gameOverPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().SetNativeSize();
    }

    public void UpdateScore(int playerScore, int botScore)
    {
        _playerScore.text = playerScore.ToString();
        _botScore.text = botScore.ToString();
    }

    public void PlayerTurnSign(bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            _scoreBoard.GetChild(0).GetComponent<CanvasGroup>().alpha = 0.5f;
            _scoreBoard.GetChild(0).GetComponent<CanvasGroup>().DOFade(1f, 1f).SetLoops(-1, LoopType.Yoyo);
            _scoreBoard.GetChild(1).GetComponent<CanvasGroup>().DOKill();
            _scoreBoard.GetChild(1).GetComponent<CanvasGroup>().DOFade(0.5f, .5f);

        }
        else
        {
            _scoreBoard.GetChild(1).GetComponent<CanvasGroup>().alpha = 0.5f;
            _scoreBoard.GetChild(1).GetComponent<CanvasGroup>().DOFade(1f, 1f).SetLoops(-1, LoopType.Yoyo);
            _scoreBoard.GetChild(0).GetComponent<CanvasGroup>().DOKill();
            _scoreBoard.GetChild(0).GetComponent<CanvasGroup>().DOFade(0.5f, .5f);
        }
    }
}

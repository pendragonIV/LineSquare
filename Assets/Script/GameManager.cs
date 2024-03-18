using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    public GameScene gameScene;
    public AI bot;
    #region Game Status

    private bool _isGameFinal = false;
    private bool _isPlayerTurn = true;

    private int _playerScore = 0;
    private int _botScore = 0;
    #endregion

    private void Start()
    {
        int mode = Mode.instance.mode;
        DotInitializer.instance.InitializingDotForGame(mode, mode);
        Checker.instance.AddAvailableDots();
        gameScene.PlayerTurnSign(_isPlayerTurn);
    }

    public void AddScore()
    {
        if (_isPlayerTurn)
        {
            _playerScore++;
        }
        else
        {
            _botScore++;
        }

        gameScene.UpdateScore(_playerScore, _botScore);
    }

    public void FinalizeGame()
    {
        _isGameFinal = true;
        bool isPlayerWin = _playerScore > _botScore;
        StartCoroutine(FinalizeGameCoroutine(isPlayerWin));
    }

    private IEnumerator FinalizeGameCoroutine(bool isPlayerWin)
    {
        yield return new WaitForSeconds(.5f);
        gameScene.FinalizeGame(isPlayerWin);
    }

    public bool IsGameFinal()
    {
        return _isGameFinal;
    }

    public bool IsPlayerTurn()
    {
        return _isPlayerTurn;
    }

    public void ChangeTurn()
    {
        _isPlayerTurn = !_isPlayerTurn;
        gameScene.PlayerTurnSign(_isPlayerTurn);
        if (!_isPlayerTurn)
        {
            bot.CalculateSolution();
        }
    }
}

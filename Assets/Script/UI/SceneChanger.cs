using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private const string MENU = "MainMenu";
    private const string GAME = "GameScene";
    private const string LEVEL_CHOOSE = "SettingScene";

    [SerializeField]
    private Transform _sceneTransition;

    private void Start()
    {
        PlayTransition();
    }

    public void PlayTransition()
    {
        _sceneTransition.GetComponent<Animator>().Play("SceneTransition");
    }

    public void ChangeToMenu()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScene(MENU));
    }

    public void ChangeToGameScene()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScene(GAME));
    }

    public void ChangeToLevels()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScene(LEVEL_CHOOSE));
    }

    private IEnumerator ChangeScene(string sceneName)
    {

        //Optional: Add animation here
        _sceneTransition.GetComponent<Animator>().Play("SceneTransitionReverse");
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadSceneAsync(sceneName);

    }
}

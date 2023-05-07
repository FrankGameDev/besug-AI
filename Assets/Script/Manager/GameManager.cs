using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogWarning("Game manager is NULL");
            return _instance;
        }
    }

    public Image fadePanel;

    [HideInInspector] public GameState currentState;

    public const string DAY_COUNT = "dayCount";


    private void Awake()
    {
        // -- Singleton
        if (_instance == null)
        {
            //First run, set the _instance
            _instance = this;
            DontDestroyOnLoad(gameObject);
            PlayerPrefs.SetInt(DAY_COUNT, 1);

        }
        else if (_instance != this)
        {
            //_instance is not the same as the one we have, destroy old one, and reset to newest one
            Destroy(_instance.gameObject);
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void UpdateGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.MENU:
                ToMenuState();
                break;
            case GameState.PLAY:
                ToDayPlayState();
                break;
            case GameState.TECNICO:
                ToTecnicoScene();
                break;
            case GameState.INCIPIT:
                SceneManager.LoadScene(0);
                break;
            case GameState.PAUSE:
                break;
            case GameState.OPTIONS:
                break;
            case GameState.LOSE:
                ToLosingScene();
                break;
            default:
                break;
        }

        currentState = newState;
    }


    private void ToMenuState()
    {
        AnimationManager.Instance.AnimationForExitScene();
        StartCoroutine(LoadSceneAsync(1));
    }



    #region PLAY

    private void ToDayPlayState()
    {
        AnimationManager.Instance.AnimationForExitScene();
        StartCoroutine(LoadSceneAsync(2));
    }



    #endregion

    #region INCIPIT

    private void ToIncipitState()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    #region TECNICO

    private void ToTecnicoScene()
    {
        StartCoroutine(LoadTecnicoSceneWithAnimation());
    }

    IEnumerator LoadTecnicoSceneWithAnimation()
    {
        AnimationManager.Instance.AnimationForEndOfTheDay();

        yield return new WaitForSeconds(4f);

        StartCoroutine(LoadSceneAsync(3));
    }

    #endregion

    #region LOSE

    private void ToLosingScene()
    {
        AnimationManager.Instance.AnimationLose();
        Destroy(LevelManager.Instance.gameObject);
        Destroy(PlayerManager.Instance.gameObject);
        Destroy(AnalyticsManager.Instance.gameObject);
        Destroy(ExternalEventManager.Instance.gameObject);
        StartCoroutine(LoadSceneAsync(4));
    }


    #endregion


    #region Fade Animation
    IEnumerator FadeIn(float speed = 0.01f)    // entri nella scena
    {
        float f = 1f;

        fadePanel.gameObject.SetActive(true);
        Color tmp = fadePanel.color;

        do
        {
            f -= .2f;
            tmp.a = f;
            fadePanel.color = tmp;
            yield return new WaitForSeconds(speed);

        } while (f > 0f);

        fadePanel.gameObject.SetActive(false);
    }

    IEnumerator FadeOut(float speed = 0.01f)   // esci dalla scena
    {
        enabled = false;

        fadePanel.gameObject.SetActive(true);

        float f = 0f;

        Color tmp = fadePanel.color;

        do
        {
            f += .2f;
            tmp.a = f;
            fadePanel.color = tmp;
            yield return new WaitForSeconds(speed);

        } while (f < 1f);

        yield return new WaitForSeconds(0.2f);
        fadePanel.gameObject.SetActive(false);
    }

    #endregion

    IEnumerator LoadSceneAsync(int scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(2f);

        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

    public enum GameState
    {
        MENU,
        PLAY,
        TECNICO,
        LOSE,
        INCIPIT,
        PAUSE,
        OPTIONS,
        TUTORIAL
    }
}

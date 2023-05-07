using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Unity.Burst.Intrinsics.X86.Avx;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogWarning("UI Manager is NULL");
            return _instance;
        }
    }

    [Header("Pannello scorrevole")]
    public GameObject[] doors;
    public Transform openDoorPos;
    public Transform closedDoorPos;

    [Header("Display")]
    public TextMeshProUGUI[] displayText;
    public TextMeshProUGUI[] timerText;
    bool[] occupiedPositions = new bool[5];
    public int ticket = 0;
    public AudioSource ding;

    public ModuleUIObject[] systems;
    private SystemController[] _systemControllers;
    public Button[] systemsBtn;
    private int _activeSystem;

    public float _doorSpeed = 1.5f;


    [Header("Pazienti")]
    public Image pazienteSpawn;

    public TextMeshProUGUI dayCounterText;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        _systemControllers = new SystemController[systems.Length];

        for(int i = 0 ; i < systems.Length; i++)
        {
            _systemControllers[i] = systems[i].GetComponentInParent<SystemController>();
        }
    }

    private void Start()
    {
        pazienteSpawn.enabled = false;
    }

    private void Update()
    {
        dayCounterText.text = "DAY: " + AnalyticsManager.Instance.dayCount;
    }
    public void Pause()
    {
        CloseDoor(1);
        LevelManager.Instance.StopGame();
        GameManager.Instance.UpdateGameState(GameManager.GameState.PAUSE);
    }

    public void Resume()
    {
        OpenDoor(1);
        LevelManager.Instance.ResumeGame();
    }

    public void ToMenu()
    {
        Debug.Log("ToMenu");
        CloseAllSystems();
        CloseDoor(0);
        LevelManager.Instance.StopGame();
        PlayerManager.Instance.ResetOverheating();
        GameManager.Instance.UpdateGameState(GameManager.GameState.MENU);
    }

    /**
     * Funzione da applicare ai pulsanti che aprono le ui dei relativi moduli
     */
    public void OpenSystem(ModuleUIObject sys)
    {
        CloseAllSystems();
        sys.gameObject.SetActive(true);
        _activeSystem = Array.FindIndex(systems, s =>  s == sys);
    }

    private void CloseAllSystems()
    {
        Array.ForEach(systems, s => s.gameObject.SetActive(false));
    }


    #region Pazienti

    public void SpawnPaziente(Image paziente)
    {
        pazienteSpawn.enabled = true;
        pazienteSpawn.sprite = paziente.sprite;
    }

    //TODO Funzione ExitPaziente ?
    public void ExitPaziente()
    {
        pazienteSpawn.enabled = false;
    }
    #endregion

    #region Estintore e Reboot

    public void Estintore()
    {
        _systemControllers[_activeSystem].ManualCooling();
    }

    public void Reboot()
    {
        _systemControllers[_activeSystem].Reboot();
    }

    #endregion

    #region Porta scorrevole


    public void OpenDoor(int i)
    {
        StartCoroutine(Opening(i));
    }

    public void CloseDoor(int i)
    {
        DisableModulesButtons();
        StartCoroutine(Closing(i));
    }

    IEnumerator Opening(int i)
    {
        
        while (doors[i].transform.position.x < openDoorPos.position.x)
        {
            doors[i].transform.position = new Vector2(doors[i].transform.position.x + (5f * Time.deltaTime), doors[i].transform.position.y);
            yield return new WaitForSeconds(0.001f);
        }

        doors[i].transform.position = openDoorPos.position;
        yield return new WaitForSeconds(0.25f);

        EnableModulesButtons();
    }

    IEnumerator Closing(int i)
    {
        while (doors[i].transform.position.x > closedDoorPos.position.x)
        {
            doors[i].transform.position = new Vector2(doors[i].transform.position.x - (5f * Time.deltaTime), doors[i].transform.position.y);
            yield return new WaitForSeconds(0.001f);
        }

        doors[i].transform.position = closedDoorPos.position;
    }

    #endregion

    public void EndOfTheDayUI(int i)
    {
        CloseAllSystems();
        CloseDoor(i);
        ExitPaziente();
        AnimationManager.Instance.DisableDayAnimator();
    }

    void EnableModulesButtons()
    {
        Array.ForEach(systemsBtn, btn => btn.interactable = true);
    }

    void DisableModulesButtons()
    {
        Array.ForEach(systemsBtn, btn => btn.interactable = false);
    }

    #region Debugger

    public void DisplayFaultSystem() // Deve SOLO mostrare il testo sulla riga
    {

        for (int i = 0; i < 5; i++) 
        {
            if (LevelManager.Instance.activeFaultCounter <= 0 && i == 0)
            {
                displayText[0].text = ">200 OK";
                timerText[0].text = "";
            }
            else if(LevelManager.Instance.activeElements.Count <= 0 && i > 0)
            {
                displayText[i].text = "";
                timerText[i].text = "";
            }
            else if ( i < LevelManager.Instance.activeElements.Count )
            {
                displayText[i].text = LevelManager.Instance.activeElements[i].text;
                timerText[i].text = (LevelManager.Instance.maxResolvingFaultTimer - LevelManager.Instance.activeElements[i].timer).ToString("00.0") + "s";
            }
            else
            {
                displayText[i].text = "";
                timerText[i].text = "";
            }
        }
    }

    #endregion
}

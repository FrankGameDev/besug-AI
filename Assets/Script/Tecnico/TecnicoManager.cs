using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TecnicoManager : MonoBehaviour
{

    private static TecnicoManager _instance;

    public static TecnicoManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogWarning("Analytics Manager is NULL");
            return _instance;
        }
    }

    [Header("Campi e References su popup di scelta potenziamento")]
    public GameObject popupSceltaChip;
    public TextMeshProUGUI testoSceltaChip;
    public Button yes;

    private ChipType _selectedChipType;

    [Header("Campi per randomizzazione power up")]
    public ChipButton powerUp;
    [Header("Campi per randomizzazione consumabili")]
    public ChipButton consumabile;

    public PowerUpChipScriptable[] chipPowerUpInfo;
    public ConsumabiliChipScriptable[] chipConsumabiliIInfo;


    private int _numConsumabiliInVendita;

    private void Awake()
    {
        // -- Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        testoSceltaChip.text = "Do you want the \"";
        popupSceltaChip.SetActive(false);

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("Eccomi");

        // randomizzo power up e consumabili
        int powerupChoice = Random.Range(1, Enum.GetValues(typeof(ChipPowerUpType)).Length);
        powerUp.chipPowerUp = (ChipPowerUpType)powerupChoice;
        powerUp.descrizioneChip = Array.Find(chipPowerUpInfo, c => c.chipPowerUp == powerUp.chipPowerUp);

        int consumabiliChoice = Random.Range(1, Enum.GetValues(typeof(ChipConsumabiliType)).Length);
        consumabile.chipConsumabili = (ChipConsumabiliType)consumabiliChoice;
        consumabile.descrizioneChip = Array.Find(chipConsumabiliIInfo, c => c.chipConsumabili == consumabile.chipConsumabili);

        _numConsumabiliInVendita = LevelManager.Instance.numeroConsumabiliInVendita;

    }


    public void MostraPopupSceltaChip(ChipType chipType, string nomePotenziamentoScelto)
    {
        testoSceltaChip.text = "Do you want the \"" + nomePotenziamentoScelto + "\"?";
        popupSceltaChip.SetActive(true);

        _selectedChipType = chipType;
    }
    public void MostraPopupSceltaChip(ChipPowerUpType chipType, string nomePotenziamentoScelto)
    {
        testoSceltaChip.text = "Do you want the \"" + nomePotenziamentoScelto + "\"?";
        popupSceltaChip.SetActive(true);

        _selectedChipType = ChipType.POWERUP;
    }
    public void MostraPopupSceltaChip(ChipConsumabiliType chipType, string nomePotenziamentoScelto)
    {
        testoSceltaChip.text = "Do you want the \"" + nomePotenziamentoScelto + "\"?";
        popupSceltaChip.SetActive(true);

        _selectedChipType = ChipType.CONSUMABILE;
    }


    public void ChooseChipAndStartNewDay()
    {
        switch (_selectedChipType)
        {
            case ChipType.RAFFREDDAMENTO:
                PlayerManager.Instance.ReduceOverheatingByPercentage(25f);
                break;
            case ChipType.POWERUP:
                switch (powerUp.chipPowerUp)
                {
                    case ChipPowerUpType.NULL:
                        break;
                    case ChipPowerUpType.TIMER_FAULT:
                        PlayerManager.Instance.AumentaTimerSoluzionePuzzle(.2f);
                        break;
                    case ChipPowerUpType.AUMENTO_CONSUMABILI:
                        LevelManager.Instance.numeroConsumabiliInVendita += 1;
                        break;
                }
                break;
            case ChipType.CONSUMABILE:
                switch (consumabile.chipConsumabili)
                {
                    case ChipConsumabiliType.NULL:
                        break;
                    case ChipConsumabiliType.NEW_COOLING_SYSTEM_9000:
                        PlayerManager.Instance.AddCoolingSys9000Use(_numConsumabiliInVendita);
                        break;
                    case ChipConsumabiliType.ANTIJAMMER:
                        PlayerManager.Instance.AddAntiJammerUse(_numConsumabiliInVendita);
                        break;
                }
                break;
        }
        GameManager.Instance.UpdateGameState(GameManager.GameState.PLAY);
    }

}

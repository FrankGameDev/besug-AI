using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
 * Viene instanziato dopo aver premuto play. 
 * Viene cancellato se torni al menu
 */
public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;

    public static PlayerManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogWarning("Level Manager is NULL");
            return _instance;
        }
    }

    [Header("SURRISCALDAMENTO")]
    public Slider barraSurriscaldamento;
    public float barSpeed;

    private float _statoSurriscaldamento;
    private float _maxSurriscaldamento = 500;

    private float _surriscaldamentoFineTimer = 100f;
    private float _surriscaldamentoMetaTimer = 100f;
    private float _surriscaldamentoErrorePuzzle = 5f;
    private float _surriscaldamentoNoFaultTouch = 5f;

    // -- POWERUP
    public int _antiJammerCount;
    public int _coolingSys9000Count;


    private void Awake()
    {
        // -- Singleton
        if (_instance == null)
        {
            //First run, set the _instance
            _instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (_instance != this)
        {
            //_instance is not the same as the one we have, destroy old one, and reset to newest one
            _statoSurriscaldamento = _instance._statoSurriscaldamento;
            _antiJammerCount  = _instance._antiJammerCount;
            _coolingSys9000Count = _instance._coolingSys9000Count;

            Destroy(_instance.gameObject);
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        barraSurriscaldamento.maxValue = _maxSurriscaldamento;
        
    }

    private void Update()
    {
        GestioneSurriscaldamento();
    }

    #region Surriscaldamento

    private void GestioneSurriscaldamento()
    {
        barraSurriscaldamento.value = _statoSurriscaldamento;
        //barraSurriscaldamento.value = Mathf.MoveTowards(barraSurriscaldamento.value, _statoSurriscaldamento, barSpeed * Time.deltaTime);

        if (_statoSurriscaldamento >= _maxSurriscaldamento)
        {
            VolumeController.instance.GameLose();
            GameManager.Instance.UpdateGameState(GameManager.GameState.LOSE);
        }
    }

    IEnumerator LoseSound()
    {
        gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(gameObject.GetComponent<AudioSource>().clip.length);
    }

    public void OverheatingAfterEndOfTimer() => _statoSurriscaldamento += _surriscaldamentoFineTimer;
    public void OverheatingAfterHalfTimer() => _statoSurriscaldamento += _surriscaldamentoMetaTimer;
    public void OverheatingPuzzleError() => _statoSurriscaldamento += _surriscaldamentoErrorePuzzle;
    public void OverheatingNoFaultTouch() => _statoSurriscaldamento += _surriscaldamentoNoFaultTouch;

    public void ReduceOverheatingByPercentage(float percentage)
    {
        Debug.Log("valore surriscaldamento: " + _statoSurriscaldamento + "; Valore da sottrarre: " + _statoSurriscaldamento * percentage / 100);
        _statoSurriscaldamento -= _statoSurriscaldamento * percentage / 100;
        Debug.Log("Valore dopo raffreddamento: " + _statoSurriscaldamento);
    }

    public void ResetOverheating()
    {
        _statoSurriscaldamento = 0;
    }
    #endregion


    #region POWERUP

    public void AumentaTimerSoluzionePuzzle(float timer)
    {
        LevelManager.Instance.maxResolvingFaultTimer += timer;
    }

    #endregion

    #region CONSUMABILI

    public void ActivateAntiJammerIfPossible()
    {
        if (_antiJammerCount == 0)
            return;

        ExternalEventManager.Instance.DisableJammer();
        _antiJammerCount -= 1;
    }

    public bool ActivateCoolingSystem9000IfPossible()
    {
        if (_coolingSys9000Count == 0)
            return false;

        _coolingSys9000Count -= 1;
        return true;

    }

    public void AddAntiJammerUse(int n) => _antiJammerCount += n;

    public void AddCoolingSys9000Use(int n) => _coolingSys9000Count += n;

    #endregion
}

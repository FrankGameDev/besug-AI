using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{

    private static LevelManager _instance;

    public static LevelManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogWarning("Level Manager is NULL");
            return _instance;
        }
    }

    [Header("Gestione fault")]
    public SystemController[] systemControllers;

    public float maxResolvingFaultTimer;
    private float _faultEventTimer = 1f;
    private float _maxFaultEventTimer;
    private float _currentFaultProbability;
    private float _initialFaultProbability = 15f;
    private float _probabilityIncrement = 1f;
    private float _probabilityIncrementMultiplier = 1f;

    [HideInInspector]public List<ActiveElement> activeElements = new List<ActiveElement>();
    //[HideInInspector]public int indexActiveElements = 0;

    [Header("Pazienti")]
    public Patient[] patients;
    private Patient _activePatient;
    private int _patientsDone;
    [HideInInspector] public float[] _patientTimer = { 15f, 20f, 30f };
    private float _currentPatientTimer, _maxCurrentPatientTimer;


    [Header("Game Loop")]
    public float _dayTimer = 60f;
    public float _currentDayTimer;


    [Header("Tecnico")]
    public int numeroConsumabiliInVendita;


    [Header("Eventi esterni")]
    private float _externalEventTimer = 5f;
    private float _maxExternalEventTimer;


    // -- UTILITY
    private int _activeModule;
    public int activeFaultCounter;
    private int _maxFault = 5;
    [HideInInspector] public bool _dayStarted;

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
            systemControllers = (systemControllers == null) ? _instance.systemControllers : systemControllers;
            maxResolvingFaultTimer = (maxResolvingFaultTimer == 0) ? _instance.maxResolvingFaultTimer : maxResolvingFaultTimer;
            numeroConsumabiliInVendita = (numeroConsumabiliInVendita == 0) ? _instance.numeroConsumabiliInVendita : numeroConsumabiliInVendita;

            Destroy(_instance.gameObject);
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        _maxFaultEventTimer = _faultEventTimer;
        _currentFaultProbability = _initialFaultProbability;
        _maxExternalEventTimer = _externalEventTimer;

        if (maxResolvingFaultTimer == 0)
            maxResolvingFaultTimer = 10;
        if (numeroConsumabiliInVendita == 0)
            numeroConsumabiliInVendita = 1;
    }


    private void Update()
    {
        if (!_dayStarted)
            return;

        UIManager.Instance.DisplayFaultSystem();

        DayTimerHandler();

        FaultHandler();

        PatientsSwitchHandler();
    }

    #region Patients

    public void PatientsSwitchHandler()
    {
        if (!CurrentPatienHandler())
            return;

        NewPatients();
    }

    private void NewPatients()
    {
        PatientsExit();

        SetPatientTimer();

        //Gestione nuovo paziente random 
        StartCoroutine(RandomPatientActivation());

    }

    IEnumerator RandomPatientActivation()
    {
        _activePatient = patients[Random.Range(0, patients.Length)];
        UIManager.Instance.SpawnPaziente(_activePatient.GetComponent<Image>());
        AnimationManager.Instance.AnimationPazienteIn();

        yield return new WaitForSeconds(2f);

        if (_activePatient.tipo == PatientsType.CONSPIRATOR)
            ExternalEventManager.Instance.Jammer(_maxCurrentPatientTimer);
    }

    private void PatientsExit()
    {
        if (_activePatient != null)
        {
            AnimationManager.Instance.AnimationPazienteOut();
            if (_activePatient.tipo == PatientsType.CONSPIRATOR)
                ExternalEventManager.Instance.DisableJammer();
            _activePatient = null;
            _patientsDone += 1;
        }
        //Animazione/suoni di uscita
    }

    private void SetPatientTimer()
    {
        if (_currentDayTimer <= _patientTimer[0])
            _currentPatientTimer = _patientTimer[0];
        else
            _currentPatientTimer = _patientTimer[Random.Range(0, _patientTimer.Length)];
        _maxCurrentPatientTimer = _currentPatientTimer;
    }

    private bool CurrentPatienHandler()
    {
        if (_currentPatientTimer <= 0)
            return true;


        _currentPatientTimer -= Time.deltaTime;


        if (_externalEventTimer <= 0)
        {
            if (_activePatient.tipo == PatientsType.NO_AI || _activePatient.tipo == PatientsType.HOT)
                ExternalEventManager.Instance.CasualEventHandler(_activePatient.tipo);

            _externalEventTimer = _maxExternalEventTimer;
        }
        else
        {
            _externalEventTimer -= Time.deltaTime;
        }
        return false;
    }

    #endregion

    #region GameLoop

    public void StartOfTheDay()
    {
        UpdateDayTime();
        _currentDayTimer = _dayTimer;
        //Spawna il primo paziente
        NewPatients();
        _dayStarted = true;
    }

    public void EndOfTheDay()
    {
        _dayStarted = false;
        if (_activePatient != null)
            AnimationManager.Instance.AnimationPazienteOut();
        UpdateDifficulty();
        AnalyticsManager.Instance.UpdateDailyAnalytics(_patientsDone);

        activeFaultCounter = 0;

        DisableAllFault();

        UIManager.Instance.EndOfTheDayUI(0);
        GameManager.Instance.UpdateGameState(GameManager.GameState.TECNICO);
    }

    private void DayTimerHandler()
    {
        if (_currentDayTimer > 0 || (_currentDayTimer <= 0 && _currentPatientTimer > 0))
        {
            _currentDayTimer -= Time.deltaTime;
            return;
        }


        EndOfTheDay();
    }

    private void UpdateDayTime()
    {
        if (AnalyticsManager.Instance.dayCount == 2 || (AnalyticsManager.Instance.dayCount - 1) % 3 == 0)
            _dayTimer += 20f;
    }


    public void StopGame()
    {
        _dayStarted = false;
    }

    public void ResumeGame()
    {
        _dayStarted = true;
    }
    #endregion


    #region Fault Handler

    public void FaultHandler()
    {
        if (!FaultTimer() || activeFaultCounter >= _maxFault)
            return;

        Debug.Log("Timer fault scaduto");

        if (!FaultProbability())
        {
            Debug.Log("Incremento probabilità Fault");
            IncrementFaultProbability();
            return;
        }


        SpawnRandomFault();
    }

    private bool FaultTimer()
    {
        if (_faultEventTimer <= 0)
        {
            _faultEventTimer = _maxFaultEventTimer;
            return true;
        }

        _faultEventTimer -= Time.deltaTime;
        return false;
    }

    #region Fault Probability 

    private bool FaultProbability() => Random.Range(0, 101) <= _currentFaultProbability;

    private void IncrementFaultProbability() => _currentFaultProbability += _probabilityIncrement * _probabilityIncrementMultiplier;

    private void ResetProbability() => _currentFaultProbability = _initialFaultProbability;

    private void UpdateDifficulty()
    {
        if (AnalyticsManager.Instance.dayCount % 5 == 0)
            _probabilityIncrementMultiplier += 1;

        _initialFaultProbability += 2.5f;
    }

    #endregion

    private void SpawnRandomFault()
    {
        Debug.Log("Scateno Fault");

        int module = Random.Range(0, systemControllers.Length);
        if (systemControllers[module].isDisabled || systemControllers[module].CheckAllPuzzleActive())
            return;

        systemControllers[module].ActivateRandomPuzzle();
        activeFaultCounter++;

        ResetProbability();
    }

    private void DisableAllFault()
    {
        Array.ForEach(systemControllers, s => s.DisableAllPuzzle());
    }

    #endregion

    #region ElementListing

    public int EnqueueElement (string text)
    {
        if (activeElements.Count >= 5)
            return -1;

        Debug.Log("E CARICO SU DISPLAY: " + text);

        UIManager.Instance.ding.Play();

        int code = Random.Range(1000, 10000);

        if (activeElements.Count <= 0)
        { activeElements.Add(new ActiveElement(maxResolvingFaultTimer, text, code)); return code; }

        while(activeElements.Exists(tmp => tmp.code == code))
            code = Random.Range(1000, 10000);
        
        activeElements.Add(new ActiveElement(maxResolvingFaultTimer, text, code));

        return code;
    }

    public void RemoveDisplayLine(int toRemove)
    {
        if (activeElements.Count >= 0)
            activeElements.Remove(activeElements.Find(el => el.code == toRemove));
    }

    #endregion

}

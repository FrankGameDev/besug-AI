using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class ExternalEventManager : MonoBehaviour
{
    private static ExternalEventManager _instance;

    public static ExternalEventManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogWarning("Event Manager is NULL");
            return _instance;
        }
    }

    private float _eventProbability = 40f;
    public bool isJammerActive;

    private bool _isCameraRotated;
    private bool _isScreenTurnedOff;

    // -- JAMMER FIELDS
    public Transform uiRotator;
    public Image fadePanel;


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
            Destroy(_instance.gameObject);
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    #region Jammer



    public void Jammer(float timer)
    {
        if (isJammerActive)
            return;

        if (timer == LevelManager.Instance._patientTimer[0])
            Debug.Log("boh");
        else if (timer == LevelManager.Instance._patientTimer[1])
            UpsideDownCamera();
        else
            StartCoroutine(ShutDownScreen());

        isJammerActive = true;

        PlayerManager.Instance.ActivateAntiJammerIfPossible();

    }

    public void DisableJammer()
    {
        Debug.Log("DisableJammer");
        isJammerActive = false;
        if (_isCameraRotated) UpsideDownCamera();
        if (_isScreenTurnedOff) ResetScreenOn();


    }

    public void UpsideDownCamera()
    {
        uiRotator.Rotate(new Vector3(uiRotator.rotation.x, uiRotator.rotation.y, 180));
        _isCameraRotated = true;
    }

    #region ShutDownScreen

    IEnumerator ResetScreenOn()
    {
        float f = fadePanel.color.a;

        fadePanel.gameObject.SetActive(true);
        Color tmp = fadePanel.color;

        do
        {
            f -= 0.02f;
            tmp.a = f;
            fadePanel.color = tmp;
            yield return new WaitForSeconds(0.01f);

        } while (f > 0f);


        _isScreenTurnedOff = false;
        fadePanel.gameObject.SetActive(false);
    }

    IEnumerator ShutDownScreen()
    {
        fadePanel.gameObject.SetActive(true);

        _isScreenTurnedOff = true;

        float f = 0f;

        Color tmp = fadePanel.color;

        do
        {
            f += 0.02f;
            tmp.a = f;
            fadePanel.color = tmp;
            yield return new WaitForSeconds(0.01f);

        } while (f < .8f);

        yield return new WaitForSeconds(0.2f);
    }


    #endregion


    #endregion

    #region Unconditional events

    public void CasualEventHandler(PatientsType tipoPaziente)
    {
        if (Random.Range(0, 101) > _eventProbability)
            return;

        if (tipoPaziente == PatientsType.HOT)
            Overheating();
        else if (tipoPaziente == PatientsType.NO_AI)
            VoltageDrop();
    }

    private void Overheating()
    {
        ChooseRandomModule(PatientsType.HOT);
    }

    private void VoltageDrop()
    {
        ChooseRandomModule(PatientsType.NO_AI);
    }

    private void ChooseRandomModule(PatientsType tipoPaziente)
    {
        SystemController[] faultManagers = LevelManager.Instance.systemControllers;
        int module = Random.Range(0, faultManagers.Length);

        if (tipoPaziente == PatientsType.HOT)
            faultManagers[module].overheating = true;
        else if (tipoPaziente == PatientsType.NO_AI)
            faultManagers[module].voltageDrop = true;

        Debug.Log("Modulo con evento attivo: " + faultManagers[module].name);

        if (!PlayerManager.Instance.ActivateCoolingSystem9000IfPossible())
            return;

        if (faultManagers[module].overheating) faultManagers[module].overheating = false;
        else if (faultManagers[module].voltageDrop) faultManagers[module].voltageDrop = false;
    }

    #endregion
}

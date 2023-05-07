using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnalyticsManager : MonoBehaviour
{

    private static AnalyticsManager _instance;

    public static AnalyticsManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogWarning("Analytics Manager is NULL");
            return _instance;
        }
    }

    public const string DAY_COUNT = "dayCount";


    [HideInInspector] public int totalPatients;
    [HideInInspector] public int maxPatientsCountInADay;
    [HideInInspector] public int dayCount = 1;


    private void Awake()
    {
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

    private void Start()
    {
        dayCount = PlayerPrefs.GetInt(DAY_COUNT, 1);
    }

    public void UpdateDailyAnalytics(int patientsDone)
    {
        dayCount += 1;
        PlayerPrefs.SetInt(DAY_COUNT, dayCount);
        totalPatients += patientsDone;
        maxPatientsCountInADay = Mathf.Max(maxPatientsCountInADay, patientsDone);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasFade : MonoBehaviour
{
    private static CanvasFade _instance;

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
        DontDestroyOnLoad(this);
    }

}

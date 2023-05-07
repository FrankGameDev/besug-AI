using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldToFill : Puzzle
{

    [Header("References")]
    public Slider fillBar;
    public Image sliderFill;
    public Button button;

    public float sliderSmoothTime;

    public Gradient gradient;

    private DetectButtonPressed _buttonHeldDetector;
    private bool _isBarFilled;

    private void Awake()
    {
        _buttonHeldDetector = GetComponentInChildren<DetectButtonPressed>();
    }

    private void Start()
    {
        fillBar.maxValue = 5f;
        fillBar.value = fillBar.minValue;
    }

    private void OnEnable()
    {
        if(_isFaultActive)
            fillBar.value = fillBar.minValue;
    }


    private void Update()
    {
        if (_isBarFilled)
            return;

        FillTheBar();
    }

    void FillTheBar()
    {
        if (!_buttonHeldDetector.isHeld)
            ChangeFillValue(fillBar.minValue);
        else
            ChangeFillValue(fillBar.maxValue);

        _isBarFilled = fillBar.value == fillBar.maxValue;
        
        if(_isBarFilled && _isFaultActive)
            _isFaultActive = false;
    }

    void ChangeFillValue(float target)
    {
        fillBar.value = Mathf.MoveTowards(fillBar.value, target, sliderSmoothTime * Time.deltaTime);

        sliderFill.color = gradient.Evaluate(fillBar.value/fillBar.maxValue);
    }

    public override void InitPuzzle()
    {
        if (!_isFaultActive)
        {
            _isFaultActive = true;
            _isBarFilled = false;
        }
    }

    public override void ResetStatus()
    {
        _isFaultActive = false;
    }
}

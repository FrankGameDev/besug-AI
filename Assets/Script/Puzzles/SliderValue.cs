using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : Puzzle
{

    public Slider ToLevel;

    int _randomized;

    private void Awake()
    {
    }

    void Update()
    {
        if (!_isFaultActive)
            return;
        else
        {
            _isFaultActive = !CheckValue();
        }
    }

    bool CheckValue()
    {
        if (ToLevel.value == _randomized) return true;
        return false;
    }

    public override void InitPuzzle()
    {
        if (!_isFaultActive)
        {
            _isFaultActive = true;
            
            ToLevel.value = 0;
            _randomized = Random.Range(0, 6);
            while(_randomized == ToLevel.value)
                _randomized = Random.Range(0, 6);

            customTerminalText = copyCustomTerminalText + _randomized.ToString();
        }
    }
    public override void ResetStatus()
    {
        _isFaultActive = false;
    }

}

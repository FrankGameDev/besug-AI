using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonValue : Puzzle
{
    public Button[] buttons;

    int _randomized = -1;

    private void Awake()
    {
    }

    public void GetBtnValue(int value) 
    {
        if (_isFaultActive && value == _randomized)
        {
            Array.ForEach(buttons, btn => btn.interactable = false);
            StartCoroutine(RightButton());
        }
        else
        {
            StartCoroutine(WrongButton(value));
            PlayerManager.Instance.OverheatingPuzzleError();
        }
    }

    IEnumerator WrongButton(int value)
    {
        buttons[value].GetComponent<Button>().image.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        buttons[value].GetComponent<Button>().image.color = Color.white;

        yield return null;
    }

    IEnumerator RightButton()
    {
        foreach (Button btn in buttons)
        {
            btn.image.color = Color.green;
            yield return new WaitForSeconds(0.05f);
            btn.image.color = Color.white;
        }

        Array.ForEach(buttons, btn => btn.interactable = true);

        _isFaultActive = false;
        yield return null;
    }

    public override void InitPuzzle()
    {
        if (!_isFaultActive) 
        {
            
            _isFaultActive = true;
            _randomized = UnityEngine.Random.Range(0, 5);
            customTerminalText = copyCustomTerminalText + _randomized.ToString();
        }
    }

    public override void ResetStatus()
    {
        _isFaultActive = false;
    }
}

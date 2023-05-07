using Mono.Cecil.Cil;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodiceNumerico : Puzzle
{
    [Header("References")]
    public Button[] buttons;
    public TextMeshProUGUI codeDisplay;

    string randomSequence = "";
    string inputSequence = "";

    private bool _lenghtCheck;

    private void Awake()
    {
    }

    private void Start()
    {
        codeDisplay.text = "";
    }



    private void Update()
    {
        if (!_isFaultActive) return;

        if (_lenghtCheck)
            CheckInputSequence();
    }


    void CheckInputSequence()
    {
        _lenghtCheck = false;
        Array.ForEach(buttons, btn => btn.interactable = false);
        if (inputSequence.Equals(randomSequence) )
        {
            SequenceOk();
            return;
        }

        SequenceError();
    }


    void SequenceError()
    {
        codeDisplay.text = "ERROR";
        inputSequence = "";
        StartCoroutine(HighlightAll(Color.red));
        PlayerManager.Instance.OverheatingPuzzleError();
    }

    void SequenceOk()
    {
        codeDisplay.text = "CORRECT";
        inputSequence = "";
        StartCoroutine(SequenceOkFlow());
    }

    public void AddToInputSequence(int index)
    {
        if (!_isFaultActive)
        {
            PlayerManager.Instance.OverheatingPuzzleError();
            return;
        }

        inputSequence += index.ToString();

        if (inputSequence.Length == randomSequence.Length) _lenghtCheck = true;
        codeDisplay.text = inputSequence;
    }

    IEnumerator SequenceOkFlow()
    {
        StartCoroutine(HighlightAll(Color.green));

        yield return new WaitForSeconds(.4f);

        _isFaultActive = false;
    }

    IEnumerator HighlightAll(Color color)
    {
        Color buttonStandardColor = buttons[0].colors.normalColor;
        Color textStandardColor = codeDisplay.color;

        Array.ForEach(buttons, b =>
        {
            ColorBlock buttonColors = b.colors;
            buttonColors.normalColor = color;
            b.colors = buttonColors;
        });

        codeDisplay.color = color;

        yield return new WaitForSeconds(.5f);

        Array.ForEach(buttons, b =>
        {
            ColorBlock buttonColors = b.colors;
            buttonColors.normalColor = buttonStandardColor;
            b.colors = buttonColors;
        });

        codeDisplay.color = textStandardColor;
        codeDisplay.text = "";

        yield return new WaitForSeconds(.01f);
        Array.ForEach(buttons, btn => btn.interactable = true);
    }

    public override void InitPuzzle()
    {
        if(!_isFaultActive)
        {
            
            _isFaultActive = true;
            for (int i = 0; i < 5; i++) 
            {
                randomSequence += UnityEngine.Random.Range(0, 10);
            }

            customTerminalText = copyCustomTerminalText + randomSequence;
        }
    }

    public override void ResetStatus()
    {
        _isFaultActive = false;
        randomSequence = "";
        inputSequence = "";
        codeDisplay.text = "";
    }
}

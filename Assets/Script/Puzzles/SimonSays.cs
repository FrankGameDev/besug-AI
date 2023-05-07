using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SimonSays : Puzzle
{
    [Header("References")]
    public Button[] buttons;
    public GameObject panelNoTouch;


    private List<int> randomSequence = new List<int>();
    private List<int> inputSequence = new List<int>();
    private float _displaySequenceTimer = 0.5f;


    private float _repeatSequenceTimer = 1.5f;
    private float _maxRepeatSequenceTimer;
    private bool _isShowingSequence;

    private void Awake()
    {
        panelNoTouch.SetActive(true);
    }

    private void Start()
    {
        inputSequence = new List<int>();
        _maxRepeatSequenceTimer = _repeatSequenceTimer;
    }


    private void Update()
    {
        if (!_isFaultActive)
            return;

        RepeatSequence();

        CheckInputSequence();
    }

    #region Sequence check

    void CheckInputSequence()
    {
        //inputSequence.ForEach(i => Debug.Log(i));
        //randomSequence.ForEach(i => Debug.Log(i));

        if (inputSequence.Count < randomSequence.Count || randomSequence.Count == 0)
            return;

        for (int i = 0; i < inputSequence.Count; i++)
        {
            if (inputSequence[i] != randomSequence[i])
            {
                SequenceError();
                return;
            }
        }

        SequenceOk();
    }


    void SequenceError()
    {
        inputSequence.Clear();
        PlayerManager.Instance.OverheatingPuzzleError();
        StartCoroutine(HighlightAllButtons(Color.red));
    }

    void SequenceOk()
    {
        StartCoroutine(HighlightAllButtons(Color.green));
        _isFaultActive = false;
    }

    public void AddToInputSequence(int index)
    {
        inputSequence.Add(index);
    }

    IEnumerator HighlightAllButtons(Color color)
    {
        Color standardColor = buttons[0].colors.normalColor;

        panelNoTouch?.SetActive(true);


        Array.ForEach(buttons, b =>
        {
            ColorBlock buttonColors = b.colors;
            buttonColors.normalColor = color;
            b.colors = buttonColors;
        });

        yield return new WaitForSeconds(.5f);

        Array.ForEach(buttons, b =>
        {
            ColorBlock buttonColors = b.colors;
            buttonColors.normalColor = standardColor;
            b.colors = buttonColors;
        });

        if (!!_isFaultActive)
            panelNoTouch?.SetActive(false);
    }

    #endregion

    #region Sequence highligthing

    void GenerateSequence()
    {

        Queue<int> sequence = new Queue<int>();

        for (int i = 0; i < 4; i++)
        {
            sequence.Enqueue(UnityEngine.Random.Range(0, buttons.Length));
        }

        randomSequence = sequence.ToList();
        randomSequence.ForEach(i => Debug.Log(i));

    }

    IEnumerator DisplaySequence(Queue<int> sequence)
    {
        int index = 0;
        Debug.Log("displaysequence");
        while (sequence.Count > 0)
        {
            index = sequence.Dequeue();
            Debug.Log(index);
            buttons[index].Select();
            buttons[index].OnSelect(null);

            yield return new WaitForSeconds(.25f);

            buttons[index].OnDeselect(null);

            yield return new WaitForSeconds(_displaySequenceTimer);

        }
        _repeatSequenceTimer = _maxRepeatSequenceTimer;
        _isShowingSequence = false;
        panelNoTouch.SetActive(false);
    }


    void RepeatSequence()
    {
        if (inputSequence.Count > 0 || _isShowingSequence) return;

        if (_repeatSequenceTimer > 0)
        {
            _repeatSequenceTimer -= Time.deltaTime;
            return;
        }
        Debug.Log("Ripeti: ");
        _isShowingSequence = true;
        StartCoroutine(DisplaySequence(new Queue<int>(randomSequence)));
    }
    #endregion




    public override void InitPuzzle()
    {
        if (!_isFaultActive)
        {
            _isFaultActive = true;
            GenerateSequence();
        }
    }

    public override void ResetStatus()
    {
        _isFaultActive = false;
        inputSequence.Clear();
    }

}



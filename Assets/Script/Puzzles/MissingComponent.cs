using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class MissingComponent : Puzzle
{

    [SerializeField]
    Image[] pieces;

    [SerializeField]
    Button[] buttons;

    [SerializeField]
    Sprite[] sprites;

    int _pieceToGuess;
    int _correctButton;

    public void CorrectSelection(int choice)
    {
        pieces[_pieceToGuess].color = Color.white;
        _isFaultActive = false;
    }

    public void WrongSelection()
    {
        PlayerManager.Instance.OverheatingPuzzleError();
    }

    void ResetItems()
    {
        pieces[_pieceToGuess].color = Color.white;

        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate { WrongSelection(); });
            btn.gameObject.SetActive(false);
        }
    }

    public override void InitPuzzle()
    {
        if (!_isFaultActive)
        {
            Debug.Log("entro");

            _pieceToGuess = UnityEngine.Random.Range(0, 5);

            _correctButton = UnityEngine.Random.Range(0, 2);

            Array.ForEach(buttons, btn => btn.gameObject.SetActive(true));

            for(int i = 0; i < buttons.Length; i++)
            {

                if (i == _correctButton)
                    buttons[i].GetComponent<Image>().sprite = sprites[_pieceToGuess];
                else
                {
                    int otherImg = UnityEngine.Random.Range(0, 3);
                    while( otherImg == _pieceToGuess) otherImg = UnityEngine.Random.Range(0, 5);
                    buttons[i].GetComponent<Image>().sprite = sprites[otherImg];
                }
            }

            buttons[_correctButton].onClick.AddListener(delegate { CorrectSelection(_correctButton); });

            pieces[_pieceToGuess].color = Color.black;
            _isFaultActive = true;
        }
    }

    public override void ResetStatus()
    {
        _isFaultActive = false;

        ResetItems();
    }

}

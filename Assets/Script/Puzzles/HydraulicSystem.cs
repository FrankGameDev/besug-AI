using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HydraulicSystem : Puzzle
{
    public Button[] Cracks;
    int _randomized;
    float _timesToClick;
    float _timesClicked;
    Color _btnColor;

    public void FixCrack()
    {

        if (_timesClicked > 2)
        {
            _timesClicked--;
            _btnColor.a = _timesClicked / _timesToClick;
            Cracks[_randomized].GetComponent<Image>().color = _btnColor;
        }
        else
        {
            _btnColor.a = 1;
            Cracks[_randomized].gameObject.SetActive(false);
            Cracks[_randomized].GetComponent<Image>().color = _btnColor;
            _isFaultActive = false;
        }
    }

    public override void InitPuzzle()
    {
        if (!_isFaultActive)
        {
            
            _isFaultActive = true;
            _randomized = Random.Range(0, 7);
            _timesClicked = _timesToClick = Random.Range(7, 13);
            Cracks[_randomized].gameObject.SetActive(true);
            _btnColor = Cracks[_randomized].GetComponent<Image>().color;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public override void ResetStatus()
    {
        gameObject.GetComponent<AudioSource>().Stop();
        _isFaultActive = false;
        
    }
}

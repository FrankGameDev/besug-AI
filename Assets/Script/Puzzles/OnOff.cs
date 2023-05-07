using UnityEngine;
using UnityEngine.UI;

public class OnOff : Puzzle
{
    [Header("Immagini pulsante")]
    public Sprite on;
    public Sprite off;

    [Header("Pulsante on/off")]
    public GameObject onOffButton;

    public override void InitPuzzle()
    {
        if(!_isFaultActive)
        {
            _isFaultActive = true;
            
            onOffButton.GetComponent<Image>().sprite = off;
        }
    }

    public void SetOn()
    {
        if (!_isFaultActive)
            return;

        onOffButton.GetComponent<Image>().sprite = on;
        _isFaultActive = false;
        //gameObject.GetComponent<AudioSource>().Play();
    }

    public override void ResetStatus()
    {
        _isFaultActive = false;
        onOffButton.GetComponent<Image>().sprite = on;

    }
}

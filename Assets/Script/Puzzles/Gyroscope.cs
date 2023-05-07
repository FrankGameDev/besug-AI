using UnityEngine;
using UnityEngine.UI;

public class Gyroscope : Puzzle
{
    
    public Slider XAxis;
    public Slider YAxis;

    public Image XColor;
    public Image YColor;

    bool _xAligned = true;
    bool _yAligned = true;

    void Start()
    {
        XAxis.value = 0.5f;
        YAxis.value = 0.5f;
    }

    private void Update()
    {
        if(!_isFaultActive) return;
        CheckAlignment();
    }

    public void CheckAlignment()
    {
        if ( XAxis.value < 0.506f && XAxis.value > 0.494f)
        {
            _xAligned = true;
            XColor.color = Color.green;
        } else XColor.color = Color.red;

        if (YAxis.value < 0.506f && YAxis.value > 0.494f)
        {
            _yAligned = true;
            YColor.color = Color.green;
        } else YColor.color = Color.red;

        if (_xAligned && _yAligned) 
        {
            Debug.Log("Allineati");
            XAxis.interactable = false;
            YAxis.interactable = false;
            _isFaultActive = false;
        }
    }

    public override void InitPuzzle()
    {
        if (!_isFaultActive)
        {
            _isFaultActive = true;
            
            float randomized = 5;
            do
            {
                randomized = Random.Range(2, 9);
                XAxis.value = randomized / 10;
                randomized = Random.Range(2, 9);
                YAxis.value = randomized / 10;

            } while (randomized == 5);
            
            XAxis.interactable = true;
            YAxis.interactable= true;

            _xAligned = false;
            _yAligned = false;

            if (XAxis.value < 0.510f && XAxis.value > 0.490f)
            {
                _xAligned = true;
                XColor.color = Color.green;
            }
            else XColor.color = Color.red;

            if (YAxis.value < 0.510f && YAxis.value > 0.490f)
            {
                _yAligned = true;
                YColor.color = Color.green;
            }
            else YColor.color = Color.red;
        }
    }

    public override void ResetStatus()
    {
        _isFaultActive = false;
        
    }
}

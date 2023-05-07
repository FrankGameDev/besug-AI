using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireTask : Puzzle
{

    public List<Color> _wireColors = new List<Color>();
    public List<Wire> _leftWires = new List<Wire>();
    public List<Wire> _rightWires = new List<Wire>();

    public Wire CurrentDraggedWire;
    public Wire CurrentHoveredWire;

    public bool IsTaskCompleted = false;

    private List<Color> _availableColors;
    private List<int> _availableLeftWireIndex;
    private List<int> _availableRightWireIndex;

    private void Start()
    {
        InitializeWires();
    }

    private void InitializeWires()
    {
        _availableColors = new List<Color>(_wireColors);
        _availableLeftWireIndex = new List<int>();
        _availableRightWireIndex = new List<int>();

        for (int i = 0; i < _leftWires.Count; i++)
        {
            _availableLeftWireIndex.Add(i);
        }

        for (int i = 0; i < _rightWires.Count; i++)
        {
            _availableRightWireIndex.Add(i);
        }

        while (_availableColors.Count > 0 &&
               _availableLeftWireIndex.Count > 0 &&
               _availableRightWireIndex.Count > 0)
        {
            Color pickedColor =
             _availableColors[Random.Range(0, _availableColors.Count)];

            int pickedLeftWireIndex = Random.Range(0,
                                      _availableLeftWireIndex.Count);
            int pickedRightWireIndex = Random.Range(0,
                                      _availableRightWireIndex.Count);
            _leftWires[_availableLeftWireIndex[pickedLeftWireIndex]]
                                              .SetColor(pickedColor);
            _rightWires[_availableRightWireIndex[pickedRightWireIndex]]
                                              .SetColor(pickedColor);

            _availableColors.Remove(pickedColor);
            _availableLeftWireIndex.RemoveAt(pickedLeftWireIndex);
            _availableRightWireIndex.RemoveAt(pickedRightWireIndex);
        }

        IsTaskCompleted = false;
        StartCoroutine(CheckTaskCompletion());

    }

    private IEnumerator CheckTaskCompletion()
    {
        while (!IsTaskCompleted)
        {
            int successfulWires = 0;

            for (int i = 0; i < _rightWires.Count; i++)
            {
                if (_rightWires[i].IsSuccess) { successfulWires++; }
            }
            if (successfulWires >= _rightWires.Count)
            {
                Debug.Log("TASK COMPLETED");
                _isFaultActive = false;
                IsTaskCompleted = true;
            }
            else
            {
                Debug.Log("TASK INCOMPLETED");
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public override void InitPuzzle()
    {
        if (!_isFaultActive)
        {
            
            _isFaultActive = true;
            InitializeWires();
        }
    }

    public override void ResetStatus()
    {
        Debug.Log("Fili disattivati");
        _isFaultActive = false;
        IsTaskCompleted = true;
        _leftWires.ForEach(w => w.lineRenderer.SetPosition(1, w.startPos));
        _rightWires.ForEach(w => w.lineRenderer.SetPosition(1, w.startPos));

    }
}

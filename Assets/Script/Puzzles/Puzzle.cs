using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    protected bool _isFaultActive;

    public string customTerminalText;
    public string copyCustomTerminalText;

    public int activeElementCode;

    public abstract void InitPuzzle();

    public abstract void ResetStatus();
    
    public bool GetStatusFault() => _isFaultActive;

    public void SetStatusFault(bool isFaultActive) { _isFaultActive = isFaultActive; }


}

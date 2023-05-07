using UnityEngine;

public class HandleValue : Puzzle
{
    public Transform Handle;
    public float angleValue;

    Vector3 _initialMousePos;

    Quaternion _initialRotation;

    float _finalRotationZ;
    float _deltaMousePos;
    float _valueToSet;          // valore che prende dal manager
    float _oldValueToSet;
    float _actualValue = 0;

    bool _isClicked;

    private void Awake()
    {
    }

    private void OnMouseDown()
    {
        _initialMousePos = Input.mousePosition;
        _isClicked = true;
        _initialRotation = Handle.rotation;
    }

    private void OnMouseDrag()
    {
        _deltaMousePos = (Input.mousePosition - _initialMousePos).x;
        Handle.rotation = _initialRotation * Quaternion.Euler(Vector3.forward * (_deltaMousePos / Screen.width) * -720);
    }

    private void OnMouseUp()
    {
        _isClicked = false;
        _finalRotationZ = Handle.eulerAngles.z;
        _actualValue = _finalRotationZ;

        if (CheckCorrectPosition())
        {
            _isFaultActive = false;
            Debug.Log("HANDLE CORRETTO");
        }
        else
            PlayerManager.Instance.OverheatingPuzzleError();
    }

    bool CheckCorrectPosition()
    {
        Debug.Log("Valore Z" + Handle.eulerAngles.z.ToString());

        if (!_isClicked)
        {
            Debug.Log("Entro Baby | Valore Handle " + Handle.eulerAngles.z.ToString() + " Valore finale " + _finalRotationZ.ToString() + " | value to set " + _valueToSet);
            if (_valueToSet == 60 && _finalRotationZ > 58f && _finalRotationZ < 62f)
                return true;
            else if (_valueToSet == 120 && _finalRotationZ > 115f && _finalRotationZ < 125f)
                return true;
            else if (_valueToSet == 180 && _finalRotationZ > 175f && _finalRotationZ < 185f)
                return true;
            else if (_valueToSet == 240 && _finalRotationZ > 235f && _finalRotationZ < 245f)
                return true;
            else if (_valueToSet == 300 && _finalRotationZ > 295f && _finalRotationZ < 305f)
                return true;
            else if (_valueToSet == 0 && _finalRotationZ > -5f && _finalRotationZ < 5f)
                return true;

            Debug.Log("HANDLE SBAGLIATO ");
            return false;

        }
        Debug.Log("HANDLE USCITO DAL CHECK ");
        return false;
    }

    public override void InitPuzzle()
    {
        if(!_isFaultActive)
        {
            
            _initialRotation = Handle.rotation;
            _initialMousePos = Vector3.zero;
            _finalRotationZ = 0;
            _deltaMousePos = 0;
            _oldValueToSet = 0;
            _valueToSet = Random.Range(0, 6);
            while (_valueToSet == _actualValue)
                _valueToSet = Random.Range(0, 6);
            _oldValueToSet = _valueToSet;
            Debug.Log("Valore to set PIENO" + _valueToSet);
            _valueToSet *= 60;
            Debug.Log("Valore to set MULTIPLIED" + _valueToSet);
            _isClicked = false;
            customTerminalText = copyCustomTerminalText + (_valueToSet/60).ToString();
            _isFaultActive = true;
        }
    }

    public override void ResetStatus()
    {
        _isFaultActive = false;
    }
}

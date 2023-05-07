using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool isHeld;


    public void OnPointerDown(PointerEventData eventData)
    {
        isHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHeld = false;
    }

}

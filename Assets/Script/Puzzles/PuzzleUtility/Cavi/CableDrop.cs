using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CableDrop : MonoBehaviour
{

    public int index;

    private void OnMouseOver()
    {
        PannelloCavi.instance.ControllaCavoCorretto(index, transform);
    }

    public void SetDropIndex(int index) => this.index = index;
}

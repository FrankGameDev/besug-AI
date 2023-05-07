using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CanvasGroup))]

public class ConnettoreCavo : MonoBehaviour
{
    LineRenderer lineRenderer;
    public RectTransform cableEnd;
    private CanvasGroup canvasGroup;

    private Vector2 _fixedEndPosition;

    private Vector2 _mousePosition;
    private bool _draggable;
    private bool _cableConnected;

    private int cavo;



    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, (Vector2)cableEnd.position);
        _fixedEndPosition = (Vector2)cableEnd.position + (Vector2.up * .1f);
        lineRenderer.SetPosition(1, _fixedEndPosition);
        lineRenderer.startWidth = .2f;
        lineRenderer.endWidth = .2f;
    }


    private void Update()
    {
        CheckMouse();
    }

    void CheckMouse()
    {

        if (!_draggable)
            return;

        if (Input.GetMouseButton(0))
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(1, _mousePosition);
        }
    }


    private void OnMouseDown()
    {
        if (_cableConnected)
            return;

        _draggable = true;
        canvasGroup.blocksRaycasts = false;
        PannelloCavi.instance.ImpostaConnettore(cavo);
    }

    private void OnMouseUp()
    {
        if (_cableConnected)
            return; 

        _draggable = false;
        canvasGroup.blocksRaycasts = true;
        lineRenderer.SetPosition(1, _fixedEndPosition);
        PannelloCavi.instance.ImpostaConnettore(-1);
    }

    public void DrawCable(Vector2 dropPoint)
    {
        lineRenderer.SetPosition(1, dropPoint);
        _cableConnected = true;
        enabled = false;
    }

    public void SetCavo(int index) => cavo = index;

    public void ResetCavo()
    {
        canvasGroup.blocksRaycasts = true;
        lineRenderer.SetPosition(1, _fixedEndPosition);
        PannelloCavi.instance.ImpostaConnettore(-1);
    }
}

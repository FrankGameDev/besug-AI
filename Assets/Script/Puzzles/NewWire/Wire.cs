using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Image))]

public class Wire : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool IsLeftWire;
    public Color CustomColor;

    private Image _image;
    public LineRenderer lineRenderer;
    public Vector3 startPos;

    private Canvas _canvas;
    private bool _isDragStarted = false;
    private WireTask _wireTask;
    public bool IsSuccess = false;

    private void Awake()
    {
        _image = GetComponent<Image>();
        lineRenderer = GetComponent<LineRenderer>();
        _canvas = GetComponentInParent<Canvas>();
        _wireTask = GetComponentInParent<WireTask>();

        lineRenderer.startWidth = .25f;
        lineRenderer.endWidth = .25f;
    }

    private void Update()
    {
        if (_isDragStarted)
        {
            Vector2 movePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        _canvas.transform as RectTransform,
                        Input.mousePosition,
                        _canvas.worldCamera,
                        out movePos);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1,
                 _canvas.transform.TransformPoint(movePos));
            startPos = _canvas.transform.TransformPoint(movePos);
        }
        else
        {
            // Hide the line if not dragging.
            // We will not hide it when it connects, later on.
            if (!IsSuccess)
            {
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
            }
        }
        bool isHovered =
          RectTransformUtility.RectangleContainsScreenPoint(
              transform as RectTransform, Input.mousePosition,
                                      _canvas.worldCamera);
        if (isHovered)
        {
            _wireTask.CurrentHoveredWire = this;
        }
    }

    public void SetColor(Color color)
    {
        _image.color = color;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        CustomColor = color;
    }
    public void OnDrag(PointerEventData eventData)
    {
        // needed for drag but not used
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsLeftWire) { return; }
        // Is is successful, don't draw more lines!
        if (IsSuccess) { return; }
        _isDragStarted = true;
        _wireTask.CurrentDraggedWire = this;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_wireTask.CurrentHoveredWire != null)
        {
            if (_wireTask.CurrentHoveredWire.CustomColor ==
                                                   CustomColor &&
                !_wireTask.CurrentHoveredWire.IsLeftWire)
            {
                IsSuccess = true;

                // Set Successful on the Right Wire as well.
                _wireTask.CurrentHoveredWire.IsSuccess = true;
            }
        }
        _isDragStarted = false;
        _wireTask.CurrentDraggedWire = null;
    }
}




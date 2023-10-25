using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick2 : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Vector2 Value { get; private set; }


    private int pointerId;
    private bool isDragging = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isDragging)
            return;

        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (pointerId != eventData.pointerId)
            return;

        Value = eventData.delta / Screen.dpi;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerId != eventData.pointerId)
            return;

        isDragging = false;
        Value = Vector2.zero;
    }

}

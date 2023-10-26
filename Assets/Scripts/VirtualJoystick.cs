using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public enum Axis
    {
        Horizontal,
        Vertical,
    }

    public Image stick;
    private float radius;

    private Vector3 originalPoint;
    private RectTransform rectTr;

    private Vector2 value;

    private int pointerId;
    private bool isDragging = false;

    public void Start()
    {
        rectTr = GetComponent<RectTransform>();
        originalPoint = stick.rectTransform.position;
        radius = rectTr.sizeDelta.x / 2;
    }

    public float GetAxis(Axis axis)
    {
        switch (axis)
        {
            case Axis.Horizontal:
                return value.x;
            case Axis.Vertical:
                return value.y;
        }
        return 0f;
    }


    public void UpdateStickPos(Vector3 screenPos)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTr, screenPos, null, out Vector3 newPoint);
        var delta = Vector3.ClampMagnitude(newPoint - originalPoint, radius);
        stick.rectTransform.position = originalPoint + delta;
        value = delta / radius;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isDragging)
            return;

        isDragging = true;
        pointerId = eventData.pointerId;
        UpdateStickPos(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (pointerId != eventData.pointerId)
            return;

        UpdateStickPos(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerId != eventData.pointerId)
            return;

        isDragging = false;
        stick.rectTransform.position = originalPoint;
        value = Vector2.zero;
    }
}

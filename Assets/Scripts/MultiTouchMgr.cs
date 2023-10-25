using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTouchMgr : MonoBehaviour
{
    public bool IsTouching { get; private set; }

    public float minZoomInch = 0.2f;
    public float maxZoomInch = 0.5f;

    private float minZoomPixel;
    private float maxZoomPixel;

    public float ZoomInch { get; private set; }

    private List<int> fingerIdList = new List<int>();
    private int primaryFingerId = int.MinValue;

    private void Awake()
    {
        minZoomPixel = minZoomInch * Screen.dpi;
        maxZoomPixel = maxZoomInch * Screen.dpi;
    }

    public void UpdatePinchToZoom()
    {
        if (fingerIdList.Count >= 2)
        {
            // [0] 1st Touch / [1] 2nd Touch
            Vector2[] prevTouchPos = new Vector2[2];
            Vector2[] currentTouchPos = new Vector2[2];
            for (int i = 0; i < 2; ++i)
            {
                var touch = Array.Find(Input.touches, x => x.fingerId == fingerIdList[i]);

                currentTouchPos[i] = touch.position;
                prevTouchPos[i] = touch.position - touch.deltaPosition;
            }
            // PrevFrame Distacne
            var prevFrameDist = Vector2.Distance(prevTouchPos[0], prevTouchPos[1]);

            // CurrFrame Distance
            var currentFrameDist = Vector2.Distance(currentTouchPos[0], currentTouchPos[1]);

            var distancePixel = prevFrameDist - currentFrameDist;
            ZoomInch = distancePixel / Screen.dpi;
            //var distanceInch = distancePixel / Screen.dpi;
            
        }

    }

    public void Update()
    {
        foreach (var touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (fingerIdList.Count == 0 && primaryFingerId == int.MinValue)
                    {
                        primaryFingerId = touch.fingerId;
                    }
                    fingerIdList.Add(touch.fingerId);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if(primaryFingerId == touch.fingerId)
                        primaryFingerId = int.MinValue;
                    fingerIdList.Remove(touch.fingerId);
                    break;
            }
        }

        if (primaryFingerId == int.MinValue && fingerIdList.Count > 0)
        {
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        // »Ÿ¿ª ¿ÃøÎ«— ¡‹ ¿Œ/æ∆øÙ ƒ⁄µÂ
#elif UNITY_ANDROID || UNITY_IOS
        UpdatePinchToZoom();
#endif
    }
}

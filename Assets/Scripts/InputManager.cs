using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    public InputManager Instance { get; private set; }

    private Vector2 mouseDownPos;
    private Vector2 mouseUpPos;
    private float minSwipeDistance = 50f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            mouseUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DetectSwipe();
        }
    }

    private void DetectSwipe()
    {
        Vector2 swipeDir = mouseUpPos - mouseDownPos;
        float swipeDistance = swipeDir.magnitude;

        if (swipeDistance < minSwipeDistance)
        {
            float swipeAngle = Mathf.Atan2(swipeDir.y, swipeDir.x) * Mathf.Rad2Deg;
            BlockMananger.Instance.MoveBlocks(swipeAngle);
        }
    }
}

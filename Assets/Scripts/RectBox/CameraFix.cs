using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFix : MonoBehaviour
{
    void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        rect.x = GameData.RectX;
        rect.y = GameData.RectY;
        rect.width = GameData.RectW;
        rect.height = GameData.RectH;

        camera.rect = rect;
    }
    void OnPreCull() => GL.Clear(true, true, Color.black);
}

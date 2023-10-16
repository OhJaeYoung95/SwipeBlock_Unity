using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    private void FixedUpdate()
    {
        transform.position += Vector3.up * Time.deltaTime * speed;
    }

    public void ReturnPoolEffect()
    {
        ObjectPoolManager.Instance.ReturnObjectPool<TextEffect>("ScoreTextPool", this);
    }
}

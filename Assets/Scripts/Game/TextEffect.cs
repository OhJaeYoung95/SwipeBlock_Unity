using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    private GameObject poolForder;
    private void FixedUpdate()
    {
        transform.position += Vector3.up * Time.deltaTime * speed;
    }

    private void OnEnable()
    {
        poolForder = GameObject.FindGameObjectWithTag("ScoreTextPool");
    }

    public void ReturnPoolEffect()
    {
        TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
        text.color = Color.white;
        gameObject.transform.SetParent(poolForder.transform);
        ObjectPoolManager.Instance.ReturnObjectPool<TextEffect>("ScoreTextPool", this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    public Button play;
    public Button option;

    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnClickPlayButton()
    {
        SceneManager.LoadScene(2);
    }

    public void OnClickOptionButton()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}

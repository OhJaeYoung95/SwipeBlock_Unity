using UnityEngine;
using TMPro;

public class TestInputManager : MonoBehaviour
{
    public TextMeshProUGUI text;


    // Update is called once per frame
    void Update()
    {
        var message = string.Empty;
        foreach(var touch in Input.touches)
        {
            //case TouchPhase.Stationary:
            //    break;
            //case TouchPhase.Canceled:
            //    break;
            //case TouchPhase.Moved:
            //    break;
            //case TouchPhase.Ended:
            //    break;
            //case TouchPhase.Began:
            //    break;
        }
        message += "\n";
        text.text = message;
    }
}

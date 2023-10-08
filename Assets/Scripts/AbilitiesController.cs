using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesController : MonoBehaviour
{
    public void OnButtonHoverEnter()
    {
        if(InputManager.Instance != null)
            InputManager.Instance.OnButtonHoverEnter();
    }

    public void OnButtonHoverExit() 
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnButtonHoverExit();
    }

    public void UseItemSlot1()
    {
        Debug.Log("UseItemSlot1");
    }
    public void UseItemSlot2()
    {
        Debug.Log("UseItemSlot2");
    }
    public void UseItemSlot3()
    {
        Debug.Log("UseItemSlot3");
    }
    public void UseSkillSlot()
    {
        Debug.Log("UseSkillSlot");
    }
}

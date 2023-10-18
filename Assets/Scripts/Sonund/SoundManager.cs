using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public static AudioClip AudioClip { get; private set; }


    public static void Init()
    {
        Resources.Load<AudioClip>("");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    void Awake()
    {
        instace = this;
    }

    public static SoundManager instace = null;

    public BgmAudio bgmAudio;
    public SfxAudio sfxAudio;
}

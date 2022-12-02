using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    void Awake()
    {
        instance = this;

        #region Valid Check
        if (bgmAudio == null)
            bgmAudio = GameObject.FindGameObjectWithTag(Tag.Sound.BGM_AUDIO).GetComponent<BgmAudio>();
        if (sfxAudio == null)
            sfxAudio = GameObject.FindGameObjectWithTag(Tag.Sound.SFX_AUDIO).GetComponent<SfxAudio>();
        #endregion
    }

    public static SoundManager instance = null;

    public BgmAudio bgmAudio;
    public SfxAudio sfxAudio;
}

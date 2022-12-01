using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sfx
{
    GunShoot, GunReload
}

public class SfxAudio : BaseAudio
{
    public void Play(Sfx sfxName, float volumeScale = 1f)
    {
        if (audioSource == null) return;
        audioSource.PlayOneShot(clips[(int)sfxName], volumeScale);
    }
}

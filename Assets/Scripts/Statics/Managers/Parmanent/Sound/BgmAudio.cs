using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Bgm
{
    Bgm1
}

public class BgmAudio : BaseAudio
{
    protected override void Start()
    {
        Play(Bgm.Bgm1);
    }

    public void Play(Bgm bgmName, bool loop = true, float volumeScale = 1f)
    {
        if (audioSource == null) return;
        audioSource.clip = clips[(int)bgmName];
        audioSource.loop = loop;
        audioSource.volume = volumeScale;
        audioSource.Play();
    }

    public void Stop()
    {
        if (audioSource == null || !audioSource.isPlaying) return;
        audioSource.Stop();
    }
}

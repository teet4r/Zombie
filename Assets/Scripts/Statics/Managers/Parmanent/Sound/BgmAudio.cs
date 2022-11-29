using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BgmAudio : MonoBehaviour
{
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    AudioSource audioSource;
}

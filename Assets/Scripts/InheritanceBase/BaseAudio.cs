using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public  class BaseAudio : MonoBehaviour
{
    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    protected virtual void Start() { }

    public bool mute
    {
        get { return audioSource.mute; }
        set { audioSource.mute = value; }
    }

    [SerializeField]
    protected AudioClip[] clips;

    protected AudioSource audioSource;
}

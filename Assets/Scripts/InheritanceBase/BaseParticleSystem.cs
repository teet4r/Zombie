using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public abstract class BaseParticleSystem : MonoBehaviour, IReturnable
{
    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        StartCoroutine(TimeOutDestroy(particleSystem.main.duration + 0.5f));
    }

    public abstract IEnumerator TimeOutDestroy(float time);
    public ParticleSystem particleSystem { get; protected set; }
}

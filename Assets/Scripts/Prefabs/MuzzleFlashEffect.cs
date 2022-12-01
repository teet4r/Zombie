using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashEffect : BaseParticleSystem
{
    public override IEnumerator TimeOutDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPool.instance.ReturnMuzzleFlashEffect(this);
    }
}

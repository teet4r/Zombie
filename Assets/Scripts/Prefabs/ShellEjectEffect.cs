using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEjectEffect : BaseParticleSystem
{
    public override IEnumerator TimeOutDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPool.instance.ReturnShellEjectEffect(this);
    }
}

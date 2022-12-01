using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    void Awake()
    {
        instance = this;
    }

    #region MuzzleFlashEffect
    public MuzzleFlashEffect GetMuzzleFlashEffect()
    {
        if (muzzleFlashEffectQ.Count == 0)
        {
            var muzzleFlashEffect = Instantiate(muzzleFlashEffectPrefab);
            muzzleFlashEffect.gameObject.SetActive(false);
            return muzzleFlashEffect;
        }
        return muzzleFlashEffectQ.Dequeue();
    }

    public void ReturnMuzzleFlashEffect(MuzzleFlashEffect muzzleFlashEffect)
    {
        if (muzzleFlashEffect == null) return;
        muzzleFlashEffect.gameObject.SetActive(false);
        muzzleFlashEffectQ.Enqueue(muzzleFlashEffect);
    }
    #endregion
    #region ShellEjectEffect
    public ShellEjectEffect GetShellEjectEffect()
    {
        if (shellEjectEffectQ.Count == 0)
        {
            var shellEjectEffect = Instantiate(shellEjectEffectPrefab);
            shellEjectEffect.gameObject.SetActive(false);
            return shellEjectEffect;
        }
        return shellEjectEffectQ.Dequeue();
    }

    public void ReturnShellEjectEffect(ShellEjectEffect shellEjectEffect)
    {
        if (shellEjectEffect == null) return;
        shellEjectEffect.gameObject.SetActive(false);
        shellEjectEffectQ.Enqueue(shellEjectEffect);
    }
    #endregion

    public static ObjectPool instance = null;

    [Header("Effect Prefabs")]
    [SerializeField]
    MuzzleFlashEffect muzzleFlashEffectPrefab;
    [SerializeField]
    ShellEjectEffect shellEjectEffectPrefab;

    Queue<MuzzleFlashEffect> muzzleFlashEffectQ = new Queue<MuzzleFlashEffect>();
    Queue<ShellEjectEffect> shellEjectEffectQ = new Queue<ShellEjectEffect>();
}

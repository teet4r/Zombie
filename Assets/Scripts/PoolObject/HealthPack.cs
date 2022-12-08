using UnityEngine;

// 체력을 회복하는 아이템
public class HealthPack : ItemObject
{
    public override void Use(GameObject target)
    {
        if (target.TryGetComponent(out LivingEntity livingEntity))
            livingEntity.RestoreHealth(heal);
        ObjectPool.instance.ReturnObject(this);
    }

    public float heal = 50; // 체력을 회복할 수치
}
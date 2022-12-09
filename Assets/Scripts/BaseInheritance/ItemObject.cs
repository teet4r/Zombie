using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : PoolObject, IItem
{
    public virtual void Use(GameObject target) { }
}

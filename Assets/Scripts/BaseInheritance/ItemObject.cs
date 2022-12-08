using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : PoolObject
{
    public virtual void Use() { }
    public virtual void Use(GameObject target) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReturnable
{
    /// <summary>
    /// This game object will destroy in time.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator TimeOutDestroy(float time);
}

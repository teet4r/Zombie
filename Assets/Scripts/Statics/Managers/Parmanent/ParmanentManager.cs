using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParmanentManager : MonoBehaviour
{
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    static ParmanentManager instance = null;
}

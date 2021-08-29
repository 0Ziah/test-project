using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float duration;

    void Start()
    {
        Invoke("DestroySelf", duration);
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}

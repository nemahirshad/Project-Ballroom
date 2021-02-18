using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public float lifeTime;
    
    private void OnEnable()
    {
        Destroy(gameObject, lifeTime);
    }
}

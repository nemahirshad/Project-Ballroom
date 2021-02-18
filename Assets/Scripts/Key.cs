using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Key : MonoBehaviour
{
    public GameObject house;
    public GameObject boy;
    
    public void Spawn()
    {
        house.SetActive(true);
        boy.SetActive(true);
    }
}

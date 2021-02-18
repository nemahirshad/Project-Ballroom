using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public List<EnemyInfo> enemies;

    public GameObject key;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].parent = this;
            enemies[i].key = key;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count <= 1)
        {
            enemies[0].hasKey = true;
        }
    }
}

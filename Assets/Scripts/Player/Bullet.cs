using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime;

    public bool isEnemy;
    public bool isPlayer;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !isEnemy)
        {
            other.gameObject.GetComponent<EnemyInfo>().TakeDamage(1);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Player") && !isPlayer)
        {
            other.gameObject.GetComponent<PlayerInfo>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}

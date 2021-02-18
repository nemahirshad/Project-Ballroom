using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public GameObject key;

    public Enemies parent;

    public int maxHealth;
    public int damage;
    public int chasers;

    public bool hasKey;
    public bool isChaser;
    public bool allDead;

    int health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (parent.enemies.Count <= chasers)
        {
            allDead = true;
        }

        if (health <= 0)
        {
            if (hasKey)
            {
                key.SetActive(true);
                key.transform.position = transform.position;
            }
            else
            {
                parent.enemies.Remove(this);
            }
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int value)
    {
        if (isChaser)
        {
            if (allDead)
            {
                health -= value;
            }
        }
        else
        {
            health -= value;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerInfo>().TakeDamage(damage);
        }
    }
}

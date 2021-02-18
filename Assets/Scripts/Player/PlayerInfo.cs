using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public UnityEvent hasKey;

    public Slider slider;

    public int maxHealth;

    int health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = health;

        if (health <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
    }

    public void TakeDamage(int value)
    {
        health -= value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            hasKey.Invoke();
            Destroy(other.gameObject);
        }
    }
}

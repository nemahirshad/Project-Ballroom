using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public GameObject heart;

    public int sceneNumber;

    public bool isHouse;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isHouse)
            {
                SceneManager.LoadScene(sceneNumber);
            }
            else
            {
                heart.SetActive(true);
            }
        }
    }
}

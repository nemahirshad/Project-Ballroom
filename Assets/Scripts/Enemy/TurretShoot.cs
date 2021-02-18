using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    public Transform pos;

    public GameObject bulletPrefab;

    public float bulletForce;
    public float fireRate;

    Vector3 position;
    Vector3 direction;

    float nextTimeToFire;

    // Update is called once per frame
    void Update()
    {
        position = pos.position;
        position.y = 0;
        direction = position - new Vector3(transform.position.x, 0, transform.position.z);

        if (Time.time >= nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + 1 / fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(direction * bulletForce, ForceMode.Impulse);
    }
}

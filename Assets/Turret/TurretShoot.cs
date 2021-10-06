using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public float fireRate = 2f;
    public AudioClip shootSFX;

    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) {
            GameObject projectile = Instantiate(projectilePrefab, 
                transform.position + transform.forward, transform.rotation) as GameObject;

            AudioSource.PlayClipAtPoint(shootSFX, transform.position);
            
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

            projectile.transform.SetParent(GameObject.FindGameObjectWithTag("Turret").transform);

            timer = fireRate;
        }
        //print(timer);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    public int startingHealth = 100;

    public int currentHealth;

    public AudioClip deadSFX;

    public Slider healthSlider;

    void Awake() {
        healthSlider = GetComponentInChildren<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeHealth(int heal) {
        TakeDamage(-heal);
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        if (currentHealth <= 0) {
            // dead!
        }

        healthSlider.value = currentHealth;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Projectile")) {
            TakeDamage(10);
        }
    }
}


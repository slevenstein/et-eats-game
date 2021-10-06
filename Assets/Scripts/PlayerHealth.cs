using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public int startingHealth = 100;

    int currentHealth;

    //public AudioClip deadSFX;

    //public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        //healthSlider.value = currentHealth;
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
            PlayerDies();
            currentHealth = 0;
        }

        //healthSlider.value = currentHealth;
    }

    void PlayerDies() {
        //AudioSource.PlayClipAtPoint(deadSFX, transform.position);
        //transform.Rotate(-90, 0, 0, Space.Self);

        Debug.Log("DEAD!");

        FindObjectOfType<LevelManager>().LevelLost();
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    public AudioClip finishSFX;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<LevelManager>().LevelBeat();
            AudioSource.PlayClipAtPoint(finishSFX, transform.position);
            Destroy(gameObject);
        }
    }
}

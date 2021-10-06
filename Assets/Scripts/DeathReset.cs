using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathReset : MonoBehaviour
{
    public CheckPoint point;
    public AudioClip deathSFX;
    public Transform PlayerLocation;


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetCheckpoint();
        }
    }

    public void ResetCheckpoint() {
        if (CheckPoint.check) {
            point.JumpToCheckpoint();
        }
    }
}

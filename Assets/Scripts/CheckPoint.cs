using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    
    public GameObject player;

   
    private Transform checkpointLocation;

    public GameObject CheckpointChecked;
    public GameObject CheckpointUnchecked;
    public static bool check = false;
    public AudioClip checkpointSFX;
    public AudioSource source;

    void Start()
    {
        checkpointLocation = CheckpointChecked.transform;
        source = gameObject.GetComponent<AudioSource>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            check = true;
            CheckpointChecked.SetActive(true);
            CheckpointUnchecked.SetActive(false);
            AudioSource.PlayClipAtPoint(checkpointSFX, CheckpointChecked.transform.position);
        }
    }

    public void JumpToCheckpoint()
    {
        player.transform.position = checkpointLocation.position;
        player.transform.rotation = checkpointLocation.rotation;
    }
}

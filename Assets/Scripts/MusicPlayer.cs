using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource cameraSource;

    void Start()
    {
        cameraSource = GetComponent<AudioSource>();
        cameraSource.Play(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

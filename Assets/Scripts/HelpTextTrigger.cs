using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpTextTrigger : MonoBehaviour
{
    public GameObject HelpText;

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HelpText.SetActive(true);
            Destroy(HelpText, 3);
            Destroy(gameObject);
        }
    }
}

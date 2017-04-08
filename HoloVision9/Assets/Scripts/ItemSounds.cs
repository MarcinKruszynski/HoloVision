using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSounds : MonoBehaviour
{

    AudioSource audioSource = null;    
    
    void Start()
    {        
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.maxDistance = 20f;
        audioSource.clip = Resources.Load<AudioClip>("Scare");        
    }

    void OnCollisionEnter(Collision collision)
    {        
        if (collision.relativeVelocity.magnitude >= 0.1f)
        {            
            audioSource.Play();
        }
    }
}

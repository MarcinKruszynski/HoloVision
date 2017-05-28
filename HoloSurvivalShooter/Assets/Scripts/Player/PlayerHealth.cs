﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    //public Slider healthSlider;
    //public Image damageImage;
    public AudioClip deathClip;
    //public float flashSpeed = 5f;
    //public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    AudioSource playerAudio;    
    PlayerShooting playerShooting;

    bool isDead;
    bool damaged;


    void Awake ()
    {
        playerAudio = GetComponent <AudioSource> ();        
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;        
    }


    void Update ()
    {
        if(damaged)
        {
            //damageImage.color = flashColour;
        }
        else
        {
            //damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

        //healthSlider.value = currentHealth;
        HealthManager.health = currentHealth;

        playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        playerShooting.DisableEffects ();        

        playerAudio.clip = deathClip;
        playerAudio.Play ();
        
        playerShooting.enabled = false;

        //RestartLevel();
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }    
}

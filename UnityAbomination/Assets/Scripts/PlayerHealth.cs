﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int startingInfection = 0;
    public int currentInfection = 0;
    public Slider infectionSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    private AudioSource playerAudio;
    private PlayerController playerController;
    private  bool isDead;
    private bool damage;

    private void Awake()
    {
        // playerAudio = GetComponent <AudioSource> ();
        // playerController = GetComponent<PlayerController>();
        currentInfection = startingInfection;

    }

    // Use this for initialization
    void Start () {
        infectionSlider = GameObject.Find("InfectionSlider").GetComponent<Slider>();
        damageImage = GameObject.Find("DamageImage").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        //flash damage image
        if (damage)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damage = false;
	}

    public void TakeDamage (int amount)
    {
        damage = true;
        currentInfection += amount;
        infectionSlider.value = currentInfection;

        // playerAudio.Play();

        if (currentInfection >= 100 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        // playerAudio.clip = deathClip;
        // playerAudio.Play();

        // playerController.enabled = false;
    }
}
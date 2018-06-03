using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int startingInfection = 0;
    public int currentInfection = 0;
    public int maxInfection = 100;
    public Slider infectionSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    public GameObject m_ExplosionPrefab;

    public GameManager gameManager;
    private AudioSource playerAudio;
    private PlayerController playerController;
    private  bool isDead;
    private bool damage;

    private AudioSource explosionAudio;

    private void Awake()
    {
        // playerAudio = GetComponent <AudioSource> ();
        // playerController = GetComponent<PlayerController>();
        currentInfection = startingInfection;

    }

    // Use this for initialization
    void Start () {
        explosionAudio = Instantiate(m_ExplosionPrefab).GetComponent<AudioSource>();
        explosionAudio.gameObject.SetActive(false);

        infectionSlider = GameObject.Find("InfectionSlider").GetComponent<Slider>();
        damageImage = GameObject.Find("DamageImage").GetComponent<Image>();
        infectionSlider.value = startingInfection;
        currentInfection = startingInfection;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

    public void AddInfection(int amount)
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
        explosionAudio.gameObject.SetActive(true);
        explosionAudio.Play();
        gameManager.StopGame("death");
        // playerAudio.clip = deathClip;
        // playerAudio.Play();

        // playerController.enabled = false;
    }
}
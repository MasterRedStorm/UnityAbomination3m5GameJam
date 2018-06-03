using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    public int m_StartingHealth = 5;
    public Slider m_Slider;
    public Image m_FillImage;
    public Image m_BackgroundImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    //public GameObject m_ExplosionPrefab;

    //private AudioSource m_ExplosionAudio;
    //private ParticleSystem m_ExplosionParticles;
    public int m_CurrentHealth;
    private bool m_Dead;

    private void Awake()
    {
        //m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        //m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        //m_ExplosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }

    public void TakeDamage(int amount)
    {
        m_CurrentHealth -= amount;

        SetHealthUI();

        if(m_CurrentHealth <= 0 && !m_Dead)
        {
            OnDeath();
        }
    }

    private void SetHealthUI()
    {
        // Adjust the value and color of the slider
        m_Slider.value = m_CurrentHealth;

        if (m_CurrentHealth == m_StartingHealth)
        {
            m_BackgroundImage.color = new Color(1, 1, 1, 0);
            m_FillImage.color = new Color(1, 1, 1, 0);
        }
        else
        {
            m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
        }
    }

    private void OnDeath()
    {
        m_Dead = true;

        //m_ExplosionParticles.transform.position = transform.position;
        //m_ExplosionParticles.gameObject.SetActive(true);

        //m_ExplosionParticles.Play();
        //m_ExplosionAudio.Play();

        gameObject.SetActive(false);
    }
}
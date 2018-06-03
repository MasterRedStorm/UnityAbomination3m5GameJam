using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    GameObject player;
    GameObject gameManager;
    GameManager gameManagerScript;
    EnemyHealth health;
    bool playerInRange;
    float timer;
    
    // Use this for initialization
    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        health = GetComponent<EnemyHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
         if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && health.m_CurrentHealth > 0)
        {
            Attack ();
        }

	}
    void Attack ()
    {
        timer = 0f;

        if(gameManagerScript.infectionLevel >= gameManagerScript.infectionLevelMax)
        {
            gameManagerScript.GetInfected(attackDamage);
        }
    }
}

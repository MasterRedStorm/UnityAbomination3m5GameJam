using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour {

    public int bossHealth;

    private GameManager gameManager;

	// Use this for initialization
	void Awake () {
        InvokeRepeating("ReduceBossHealth", 5f, 5f);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        bossHealth = 15;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ReduceBossHealth ()
    {
        bossHealth -= 1;
        if (bossHealth <= 0)
        {
            // gameManager.StopGame("You Won");
        }
    }
}

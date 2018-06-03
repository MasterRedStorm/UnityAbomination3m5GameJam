using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour {

    public int bossHealth = 5;

    private GameManager gameManager;

	// Use this for initialization
	void Start () {
        InvokeRepeating("ReduceBossHealth", 2f, 2f);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ReduceBossHealth ()
    {
        bossHealth -= 1;
        if (bossHealth <= 0)
        {
            gameManager.StopGame("You Won");
        }
    }
}

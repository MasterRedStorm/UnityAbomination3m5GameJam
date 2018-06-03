using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    // Base speed game for e.g. 
    public float baseSpeed = 2;
    public int travelDistance = 0;
    public int level = 1;
    public GameObject player;
    public GameObject enemy;
    public GameObject[] enemies;
    public GameObject background;

	// Use this for initialization
	void Start () {
        InitGame();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitGame()
    {
        CreatePlayer();
        CreateBackground();
    }

    void CreatePlayer()
    {
        GameObject tmpPlayer = Instantiate(player, new Vector3( -9f,0f,0f), Quaternion.identity);
        
    }

    void CreateBackground()
    {
        GameObject tmpBackground = Instantiate(background);
        
    }

    void CreateEnemy()
    {
    }
}
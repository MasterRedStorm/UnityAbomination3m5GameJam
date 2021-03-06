﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    // Base speed game for e.g. 
    public float baseSpeed = 2;
    public int travelDistance = 0;
    public int level = 1;
    public GameObject player;
    public GameObject enemy;
	public GameObject flow;
    public GameObject background;
    public GameObject boss;

    public int score;

    // Start / End
    public int levelStartDelay = 3;
    public float timeleft;
    public bool gameIsRunning;
    public bool bossCreated = false;
    public int bossAtTravelDistance = 30;

    // UI Elements
    public GameObject Scoreboard;
    public GameObject levelImage;
    public GameObject startText;
    public Text finText;
    public Text timeScore;
    public Text infectedScore;
    public Text finalScore;

    // Sound Element
    public AudioSource myAudio;

    private bool hadEnemy = false;
    private GameObject tmpPlayer;

    // Use this for initialization
    void Start () {
        gameIsRunning = false;
        InitGame();
    }

    // Update is called once per frame
    void Update ()
	{
		const float avgFramesBetweenEnemies = 20.0f;
		const int maxNumberOfEnemies = 15;
		const float avgFramesBetweenFlows = 250.0f;
		const int maxNumberOfFlows = 5;

        if (gameIsRunning == false)
        {
            timeleft -= Time.deltaTime;
            if (Mathf.RoundToInt(timeleft) == 0)
            {
                Debug.Log("start");
                StartGame();
            }
            else
            {
                startText.GetComponent<Text>().text = "Start in " + Mathf.RoundToInt(timeleft);
            }
            return;
        }
        // Update traveldistance
        travelDistance = Mathf.RoundToInt(Time.time * baseSpeed);

        var enemyCount = GameObject.FindObjectsOfType<EnemyController>().Length;

		if (Random.value < 1 / avgFramesBetweenEnemies && enemyCount < maxNumberOfEnemies)
		{
			CreateEnemy();
		}

		var flowCount = FindObjectsOfType<FlowScript>().Count(f => f.IsRandom);
		if (Random.value < 1 / avgFramesBetweenFlows && flowCount < maxNumberOfFlows)
		{
			CreateFlow();
		}

        // Create Boss
        if(bossCreated == false && travelDistance > bossAtTravelDistance)
        {
            CreateBoss();
        }
	}
	private void InitGame()
    {
        CreatePlayer();
        score = 0;
        timeleft = (float)levelStartDelay;
        Scoreboard = GameObject.Find("Scoreboard");
        levelImage = GameObject.Find("levelImage");
        startText = GameObject.Find("Starttext");
        finText = GameObject.Find("Fintext").GetComponent<Text>();
        timeScore = GameObject.Find("TimeScore").GetComponent<Text>();
        infectedScore = GameObject.Find("InfectionScore").GetComponent<Text>();
        finalScore = GameObject.Find("FinalScoreScore").GetComponent<Text>();
        startText.GetComponent<Text>().text = "Start in " + Mathf.RoundToInt(timeleft);
        myAudio = GetComponent<AudioSource>();

        Scoreboard.SetActive(false);
    }

    private void CreatePlayer()
    {
        tmpPlayer = Instantiate(player, new Vector3( -9f,0f,0f), Quaternion.identity);
    }

    void CreateEnemy()
    {
	    var topRight = GetScreenEdgesInWorldPoints()[1];
		
		var posX = topRight.x + 1;
		var posYMax = Mathf.Abs(topRight.y) - 1;
	
		var newEnemy = Instantiate(enemy, new Vector3(posX, Random.Range(-posYMax, posYMax), 0), Quaternion.identity).GetComponent<EnemyController>();
	    if (Random.value > 0.8f)
	    {
		    newEnemy.FollowPlayer = true;
		    newEnemy.GetComponent<Animator>().SetBool("FollowPlayer", true);
	    }
    }

    private void CreateBoss()
    {
        Instantiate(boss);
        bossCreated = true;
    }

	private void CreateFlow()
	{
		var rect = GetScreenRect();
		
		var newFlow = Instantiate(
			flow,
			new Vector3(
				Random.Range(rect.xMin + 1, rect.xMax - 1),
				Random.Range(rect.yMin + 1, rect.yMax - 1),
				0
			),
			Quaternion.identity
		).GetComponent<FlowScript>();
			
		newFlow.IsRandom = true;
	}

	/// <summary>
	/// 0 topLeft
	/// 1 topRight
	/// 2 bottomRight
	/// 3 bottomLeft
	/// </summary>
	/// <returns></returns>
	private static Vector3[] GetScreenEdgesInWorldPoints()
	{
		var camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		var topLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
		var topRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
		var bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
		var bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));

		return new List<Vector3> {topLeft, topRight, bottomRight, bottomLeft}.ToArray();
	}

	public static Rect GetScreenRect()
	{
		var corners = GetScreenEdgesInWorldPoints();
		return new Rect(
			corners[0].x,
			corners[0].y,
			corners[2].x - corners[0].x,
			corners[2].y - corners[0].y
		);
	}

	public static Rect GetFieldRectForObject(Collider2D body)
	{
		var rect = new Rect(GetScreenRect());
		
		rect.yMin += 0.5f;

		rect.xMin += body.bounds.size.x / 2;
		rect.xMax -= body.bounds.size.x / 2;
		rect.yMin += body.bounds.size.y / 2;
		rect.yMax -= body.bounds.size.y / 2;

		return rect;
	}

    public void StartGame()
    {
        levelImage.SetActive(false);
        Scoreboard.SetActive(true);
        startText.SetActive(false);
        myAudio.enabled = true;
        gameIsRunning = true;
    }

    public void StopGame(string reason)
    {
        GetComponent<AudioSource>().Stop();
        score += Mathf.RoundToInt(GetComponent<MyTimer>().time); // reduce time spend each second -10 points
        score -= tmpPlayer.GetComponent<PlayerHealth>().currentInfection / 2; // reduce by infection take
        levelImage.SetActive(true);
        if (reason == "death")
        {
            finText.text = "You are Dead!";
        }
        else
        {
            finText.text = "You Won!";
        }

        timeScore.text = Mathf.RoundToInt(GetComponent<MyTimer>().time).ToString();
        infectedScore.text = (-tmpPlayer.GetComponent<PlayerHealth>().currentInfection / 2).ToString();
        finalScore.text = score.ToString();
        gameIsRunning = false;
        tmpPlayer.SetActive(false);
        enabled = false;
    }
}
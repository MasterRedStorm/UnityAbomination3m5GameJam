using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    // Base speed game for e.g. 
    public float baseSpeed = 2;
    public int travelDistance = 0;
    public int level = 1;
    public int infectionLevelMax = 100;
    public int infectionLevel = 0;
    public GameObject player;
    public GameObject enemy;
	public GameObject flow;
    public GameObject background;

	private bool hadEnemy = false;

	// Use this for initialization
	void Start () {
        InitGame();

	}
	
	// Update is called once per frame
	void Update ()
	{
		const float avgFramesBetweenEnemies = 20.0f;
		const int maxNumberOfEnemies = 10;
		const float avgFramesBetweenFlows = 300.0f;
		const int maxNumberOfFlows = 5;
		
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
	}

	private void InitGame()
    {
        CreatePlayer();
	    
    }

	private void CreatePlayer()
    {
        GameObject tmpPlayer = Instantiate(player, new Vector3( -9f,0f,0f), Quaternion.identity);
        
    }

    void CreateEnemy()
    {
	    var topRight = GetScreenEdgesInWorldPoints()[1];
		
		var posX = topRight.x + 1;
		var posYMax = Mathf.Abs(topRight.y) - 1;
	
		Instantiate(enemy, new Vector3(posX, Random.Range(-posYMax, posYMax), 0), Quaternion.identity);
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
		);
			
		newFlow.GetComponent<FlowScript>().IsRandom = true;
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

    public void GetInfected(int amount)
    {

    }
}
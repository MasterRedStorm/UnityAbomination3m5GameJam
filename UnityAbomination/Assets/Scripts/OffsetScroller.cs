using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScroller : MonoBehaviour {

    public float ScrollSpeed;
    public Renderer rend;
    public float baseSpeed = 1f;
    private GameManager gameManager;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }
	
	// Update is called once per frame
	void Update () {

        float offset = Time.time * ScrollSpeed * gameManager.baseSpeed;
        Vector2 currentyOffset = rend.material.GetTextureOffset("_MainTex");
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, currentyOffset.y));
    }
}

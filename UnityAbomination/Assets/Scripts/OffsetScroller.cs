﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScroller : MonoBehaviour {

    public float ScrollSpeed;
    public Renderer rend;
    public float baseSpeed = 1f;
    public GameObject mygameManager;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {

        
        float offset = Time.time * ScrollSpeed * baseSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgrounds : MonoBehaviour {

    public GameObject backgroundFab;
    public GameObject midgroundFab;
    public GameObject forgroundFab;

    public GameObject background;
    public GameObject midground;
    public GameObject forground;


	// Use this for initialization
	void Start () {
        background = Instantiate(backgroundFab);
        midground = Instantiate(midgroundFab);
        forground = Instantiate(midgroundFab);
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}

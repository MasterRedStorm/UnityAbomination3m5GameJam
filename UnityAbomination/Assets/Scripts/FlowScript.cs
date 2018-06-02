using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowScript : MonoBehaviour
{

	public float Strength;
	
	// Use this for initialization
	private void Start ()
	{
		var strength = Random.Range(1.0f, 2.0f);
		GetComponent<Transform>().Rotate(0.0f, 0.0f, Random.Range(0.0f, 1.0f) * 360);
		GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, strength * 0.5f);

		Strength = strength;// * 5.0f;
	}
	
	
}

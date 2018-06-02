using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	public int Life;

	public float Speed;

	public float MaxSpeed;

	public float Acceleration;

	public float MaxOffset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void FixedUpdate()
	{
		var player = GetComponent<Rigidbody>();

		var moveUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
		var moveDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

		var anyDirectionKey = moveUp != moveDown;

		var direction = !anyDirectionKey
			? 0.0f
			: (
				moveUp
					? 1.0f
					: -1.0f
			)
		;

		if (Mathf.Abs(Speed) < 0.1f)
		{
			Speed = 0.0f;
		}
		else
		{
			if (Mathf.Approximately(direction, 0.0f))
			{
				direction = -0.33f * Speed / Mathf.Abs(Speed);
			}
		}
		
		Speed = Mathf.Clamp(Speed + direction * Acceleration, -MaxSpeed, MaxSpeed);

		var movement = new Vector3(0.0f, Speed, 0.0f);
		
		player.velocity = movement;
		
		player.position = new Vector3(
			player.position.x,
			Mathf.Clamp(player.position.y, -MaxOffset, MaxOffset),
			player.position.z
		);
	}
}

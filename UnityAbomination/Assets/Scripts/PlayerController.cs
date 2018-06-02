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

internal enum DirectionForwardBackward
{
    Backward = -1,
    None,
    Forward
}

internal enum DirectionUpDown
{
    Down = -1,
    None,
    Up
}

public class PlayerController : MonoBehaviour
{
    public int Life;

    public float SpeedUpDown;
    
    public float SpeedForward;

    public float MaxSpeed;

    public float Acceleration;

    public float MaxOffset;

    public int inEnemyTrigger = 0;    

    private void FixedUpdate()
    {
        var player = GetComponent<Rigidbody2D>();
        
        var directionLeftRight = GetKeysDirectionForwardBackward();
        var directionUpDown = GetKeysUpDownDirection();

        var moveUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        var moveDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        SpeedUpDown = ChangeSpeed(SpeedUpDown, (int)directionUpDown);
        SpeedForward = ChangeSpeed(SpeedForward, (int)directionLeftRight);

        var movement = new Vector2(SpeedForward, SpeedUpDown);

        player.velocity = movement;

        player.position = new Vector2(
            Mathf.Clamp(player.position.x, -MaxOffset, MaxOffset),
            Mathf.Clamp(player.position.y, -MaxOffset, MaxOffset)
        );
        if (inEnemyTrigger > 0)
        {
            Debug.Log("ouch");
        }
    }
   
    private static DirectionForwardBackward GetKeysDirectionForwardBackward()
    {
        var moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        var moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        var leftRightMotionKey = moveLeft != moveRight;

        return !leftRightMotionKey
                ? DirectionForwardBackward.None
                : (moveLeft
                    ? DirectionForwardBackward.Backward
                    : DirectionForwardBackward.Forward
                )
            ;
    }

    private static DirectionUpDown GetKeysUpDownDirection()
    {
        var moveUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        var moveDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        var upDownMotionKey = moveUp != moveDown;
        
        return !upDownMotionKey
            ? DirectionUpDown.None
            : (moveUp
                ? DirectionUpDown.Up
                : DirectionUpDown.Down
            );
    }
    
    private float ChangeSpeed(float speed, int currentDirection)
    {
        float accelerationFactor = currentDirection;
        
        if (Mathf.Abs(speed) < 0.1f)
        {
            speed = 0.0f;
        }
        else
        {
            if (Mathf.Approximately(currentDirection, 0.0f))
            {
                accelerationFactor = -0.33f * speed / Mathf.Abs(speed);
                speed += currentDirection * Acceleration;
            }
        }
        
        var updatedSpeed = Mathf.Clamp(speed + accelerationFactor * Acceleration, -MaxSpeed, MaxSpeed);

        return updatedSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            inEnemyTrigger++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            inEnemyTrigger--;
        }
    }
}
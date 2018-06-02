using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public float DragFactor;

    public float MaxOffset;

    public int inEnemyTrigger = 0;

    public int InfectionRate = 10;

    private void FixedUpdate()
    {
        var player = GetComponent<Rigidbody2D>();

        var directionX = CheckKeyDirectionX();
        var directionY = CheckKeysDirectionY();

        SpeedUpDown = ChangeSpeed(SpeedUpDown, (int) directionY, FlowPassages.Count == 0);
        SpeedForward = ChangeSpeed(SpeedForward, (int) directionX, FlowPassages.Count == 0);

        var aggregatedVector = FlowPassages.Aggregate(
            new Vector2(),
            (vector, gameObject) =>
            {
                var eulerRotation = gameObject.transform.rotation.eulerAngles.z / 360 * 2 * Mathf.PI;
                var x = -Mathf.Sin(eulerRotation);
                var y = Mathf.Cos(eulerRotation);
                
                var directionalVector = new Vector2(
                    x * gameObject.Strength,
                    y * gameObject.Strength
                );
                return vector + directionalVector;
            }
        );

        SpeedForward += aggregatedVector.x;
        SpeedUpDown += aggregatedVector.y;
        
        var movement = new Vector2(SpeedForward, SpeedUpDown);
        
        player.velocity = movement;

        var aspect = (float) Screen.width / (float) Screen.height;

        var offsetX = MaxOffset * 0.95f * aspect;
        var offsetY = MaxOffset * 0.95f;

        player.position = new Vector2(
            Mathf.Clamp(player.position.x, -offsetX, offsetX),
            Mathf.Clamp(player.position.y, -offsetY, offsetY)
        );
        if (inEnemyTrigger > 0)
        {
            Debug.Log("ouch");
        }
    }

    private static DirectionForwardBackward CheckKeyDirectionX()
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

    private static DirectionUpDown CheckKeysDirectionY()
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

    private float ChangeSpeed(float speed, int currentDirection, bool applySlowdown)
    {
        float accelerationFactor = currentDirection;

        if (Mathf.Abs(speed) < Acceleration / 2)
        {
            speed = 0.0f;
        }
        else
        {
            if (Mathf.Approximately(currentDirection, 0.0f))
            {
                accelerationFactor = -DragFactor * speed / Mathf.Abs(speed);
                speed += currentDirection * Acceleration;
            }
        }

        var updatedSpeed = Mathf.Clamp(speed + accelerationFactor * Acceleration, -MaxSpeed, MaxSpeed);

        return updatedSpeed;
    }

    public List<FlowScript> FlowPassages = new List<FlowScript>();

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                inEnemyTrigger++;
                break;
            case "FlowPassage":
                FlowPassages.Add(other.gameObject.GetComponent<FlowScript>());
                break;
            default:
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                inEnemyTrigger--;
                break;
            case "FlowPassage":
                FlowPassages.Remove(other.gameObject.GetComponent<FlowScript>());
                break;
            default:
                break;
        }
    }
}
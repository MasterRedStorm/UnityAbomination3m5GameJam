using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public float MaxOffset;

    public int inEnemyTrigger = 0;

    public int InfectionRate = 10;

    private void FixedUpdate()
    {
        var player = GetComponent<Rigidbody2D>();

        var directionX = CheckKeyDirectionX();
        var directionY = CheckKeysDirectionY();

        var nonLinearMovement = GetComponent<NonLinearMovement>();

        // clean up "dead" flows
        FlowPassages = FlowPassages.Where(f => f.isActiveAndEnabled).ToList();
        nonLinearMovement.Move(ref player, new Vector2((int) directionX, (int) directionY), FlowPassages);

        var aspect = (float) Screen.width / (float) Screen.height;

        var rect = GameManager.GetFieldRectForObject(GetComponent<BoxCollider2D>());
        
        player.position = new Vector2(
            Mathf.Clamp(player.position.x, rect.xMin, rect.xMax),
            Mathf.Clamp(player.position.y, rect.yMin, rect.yMax)
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
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NonLinearMovement : MonoBehaviour
{
    public int DirectionX;

    public int DirectionY;

    public int FacingDirection;

    public float MaxSpeed;

    public float Acceleration;

    public float DragFactor;

    public Vector2 Velocity;

    public void Move(ref Rigidbody2D subject, Vector2 direction, List<FlowScript> flowPassages)
    {
        var speedX = ChangeSpeed(Velocity.x, Mathf.FloorToInt(direction.x), flowPassages.Count == 0);
        var speedY = ChangeSpeed(Velocity.y, Mathf.FloorToInt(direction.y), flowPassages.Count == 0);

        var ownMovementVector = new Vector2(speedX, speedY);

        var appliedMovementVector = flowPassages.Aggregate(
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

        Velocity = ownMovementVector + appliedMovementVector;
        subject.velocity = Velocity;
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
}
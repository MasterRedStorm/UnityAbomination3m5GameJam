using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;

public class EnemyController : MonoBehaviour
{

	private bool _hasBeenOnField = false;

	public bool FollowPlayer = false;
	
	// Update is called once per frame
	void Update()
	{
		var body = GetComponent<Rigidbody2D>();
		var movement = GetComponent<NonLinearMovement>();
		if (movement != null)
		{
			// clean up "dead" flows
			FlowPassages = FlowPassages.Where(f => f.isActiveAndEnabled).ToList();
			movement.Move(
				ref body,
				FollowPlayer
					? new Vector2(-1.0f, 0.5f * (float)GetVectorComponentForPlayer().y)
					: Vector2.left,
				FlowPassages
			);
		}
		else
		{
			Debug.Log("No movement script attached");
		}

		var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		if (gameManager == null) return;

		var rect = GameManager.GetFieldRectForObject(GetComponent<BoxCollider2D>());
		var containsBody = rect.Contains(body.position);

		if (!_hasBeenOnField)
		{
			if (containsBody)
			{
				_hasBeenOnField = true;
			}
		}
		else
		{
			if (rect.xMin > body.position.x + 2 * GetComponent<BoxCollider2D>().bounds.size.x)
			{
				Destroy(gameObject);
			}

			if ((rect.yMin > body.position.y && movement.Velocity.y < 0) || (rect.yMax < body.position.y && movement.Velocity.y > 0))
			{
				movement.Velocity.y *= -1;
			}
		}
	}
	
	public List<FlowScript> FlowPassages = new List<FlowScript>();

	void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.gameObject.tag)
		{
			case "FlowPassage":
				var angle = other.gameObject.GetComponent<Transform>().rotation.eulerAngles.z;
				if (angle > 180)
				{
					GetComponent<NonLinearMovement>().Velocity *= 0.25f;
				}
				
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
			case "FlowPassage":
				FlowPassages.Remove(other.gameObject.GetComponent<FlowScript>());
				break;
			default:
				break;
		}
	}

	private Vector2 GetVectorComponentForPlayer()
	{
		var playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
		if (playerTransform == null)
		{
			return Vector2.zero;
		}
		var ownTransform = GetComponent<Transform>();
		var ownCollider = GetComponent<BoxCollider2D>();
		var ownMovement = GetComponent<NonLinearMovement>();

		if (ownTransform.position.y > playerTransform.position.y + 0.8f * ownCollider.bounds.size.y)
		{
			return Vector2.down;
		}
		else if (ownTransform.position.y < playerTransform.position.y - 0.8f * ownCollider.bounds.size.y)
		{
			return Vector2.up;
		}
		else
		{
			if (Mathf.Abs(ownMovement.Velocity.y) > 1)
			{
				return Mathf.Sign(ownMovement.Velocity.y) > 1
					? Vector2.down
					: Vector2.up
				;
			}

			ownMovement.Velocity.y = 0;
			return Vector2.zero;
		}
	}
}

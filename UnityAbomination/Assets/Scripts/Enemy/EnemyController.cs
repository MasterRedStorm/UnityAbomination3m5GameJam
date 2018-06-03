using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour
{

	private bool _hasBeenOnField = false;
	
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
				Vector2.left,
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
}

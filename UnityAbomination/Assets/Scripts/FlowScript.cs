using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlowScript : MonoBehaviour
{
	private const float StrengthRangeMin = 0.225f;

	private const float StrengthRangeMax = 0.55f;

	private SpriteRenderer renderer;
	
	public List<Sprite> sprites = new List<Sprite>();

	public float Strength;

	private float spriteIndex = 0;

	public float Scale;
	
	public float? Direction;

	public bool IsRandom;

	private int _iterationCounter = 0;
	
	// Use this for initialization
	private void Start ()
	{
		var strength = Random.Range(StrengthRangeMin, StrengthRangeMax);
		GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, NormalizeToColorAlphaComponent(strength));
		var transform = GetComponent<Transform>();

		if (!Direction.HasValue)
		{
			Direction = Random.Range(0.0f, 1.0f) * 360;
		}
		transform.Rotate(0.0f, 0.0f, Direction.Value);

		if (Mathf.Approximately(Scale, 0.0f))
		{
			Scale = Random.Range(0.75f, 3.0f);
		}
		transform.localScale = new Vector3(Scale, Scale, 1);

		Strength = strength;

		renderer = GetComponent<SpriteRenderer>();
		sprites.AddRange(Resources.LoadAll<Sprite>("wind"));
	}

	private void Update()
	{
		renderer.sprite = sprites[Mathf.FloorToInt(spriteIndex)];
		spriteIndex = (spriteIndex + 0.25f) % sprites.Count;
		if (Mathf.Approximately(spriteIndex, 0.0f))
		{
			if (_iterationCounter == 2)
			{
				Destroy(gameObject);	
			}
			else
			{
				_iterationCounter++;
			}
		}
	}

	private static float NormalizeToColorAlphaComponent(float strength)
	{
		return ((strength - StrengthRangeMin ) / (StrengthRangeMax - StrengthRangeMin) + 1.0f) / 2;
	}
	
}

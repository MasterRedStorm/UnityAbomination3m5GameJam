using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlowScript : MonoBehaviour
{
	private const float StrengthRangeMin = 0.2f;

	private const float StrengthRangeMax = 0.55f;

	private SpriteRenderer renderer;
	
	public List<Sprite> sprites = new List<Sprite>();

	public float Strength;

	private float spriteIndex = 0;
	
	// Use this for initialization
	private void Start ()
	{
		var strength = Random.Range(StrengthRangeMin, StrengthRangeMax);
		GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, NormalizeToColorAlphaComponent(strength));
		GetComponent<Transform>().Rotate(0.0f, 0.0f, Random.Range(0.0f, 1.0f) * 360);

		Strength = strength;

		renderer = GetComponent<SpriteRenderer>();
		sprites.AddRange(Resources.LoadAll<Sprite>("wind"));
	}

	private void Update()
	{
		renderer.sprite = sprites[Mathf.FloorToInt(spriteIndex)];
		spriteIndex = (spriteIndex + 0.25f) % sprites.Count;
	}

	private static float NormalizeToColorAlphaComponent(float strength)
	{
		return ((strength - StrengthRangeMin ) / (StrengthRangeMax - StrengthRangeMin) + 1.0f) / 2;
	}
	
}

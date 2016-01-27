using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The projectile wount actualy hit the enemy
/// The projectile is just an illustion
/// The target will be hit if it is close enough to the player and there are no obstacles
/// </summary>
public class Projectile : MonoBehaviour {
	
	public float speed = 10;
	public float damage = 1;
	public float lifeTime = .2f; // Must be relative depending target distance
	float skinWidth = .1f; // Used to fix collision detection in very close range
	public Transform target;
	public event Action OnDestinationReach;

	void Update()
	{
		transform.position = MoveTowardsCustom(transform.position, target.position, speed * Time.deltaTime);
	}

	Vector2 MoveTowardsCustom(Vector2 current, Vector2 target, float maxDistanceDelta)
	{
		Vector2 bending = Vector2.down;
		Vector2 a = target - current;
		float magnitude = a.magnitude;
		if (magnitude <= maxDistanceDelta || magnitude == 0f)
		{
			Destroy(gameObject, lifeTime);
			return target;
		}

		Vector2 currentPos = current + a / magnitude * maxDistanceDelta;

		currentPos.x += Mathf.Sin(Mathf.Clamp01(Time.time) / 50) * Mathf.PI;
		currentPos.y += Mathf.Sin(Mathf.Clamp01(Time.time) / 50) * Mathf.PI;

		return currentPos;
    }
}

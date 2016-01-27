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
	public Enemy target;
	private bool alreadyHit;

	public event Action OnDestinationReach;

	public float Magnitude { get; set; }

	void Update()
	{
		if (target != null)
		{
			transform.position = MoveTowardsCustom(transform.position, target, speed * Time.deltaTime);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	Vector2 MoveTowardsCustom(Vector2 current, Enemy target, float maxDistanceDelta)
	{
		Vector2 bending = Vector2.down;
		Vector2 targetPosition = (Vector2)target.transform.position;
        Vector2 a = targetPosition - current;
		float magnitude = a.magnitude;
		if (magnitude <= maxDistanceDelta || magnitude == 0f)
		{
			Destroy(gameObject, lifeTime);

			if (!alreadyHit)
			{
				OnHitObject(target.gameObject);
				alreadyHit = true;
			}


			return targetPosition;
		}

		Vector2 currentPos = current + a / magnitude * maxDistanceDelta;

		currentPos.x += Mathf.Sin(Mathf.Clamp01(Time.time) / 50) * Mathf.PI;
		currentPos.y += Mathf.Sin(Mathf.Clamp01(Time.time) / 50) * Mathf.PI;

		return currentPos;
    }

	void OnHitObject(GameObject hitTarget)
	{
		IDamageable damageableObject = hitTarget.GetComponent<IDamageable>();
		if (damageableObject != null)
		{
			damageableObject.TakeHit(damage, transform.position, transform.position.normalized);
		} 
	}
}

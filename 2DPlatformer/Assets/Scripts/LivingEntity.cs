using UnityEngine;
using System.Collections;
using System;

public class LivingEntity : MonoBehaviour, IDamageable
{
	public float startingHealth;
	public float health { get; protected set; }
	protected bool dead;

	public event Action OnDeath;

	protected virtual void Start()
	{
		health = startingHealth;
	}

	public virtual void TakeHit(float damage, Vector2 hitPoint, Vector2 hitDirection)
	{
		// Do some stuff here with hit variable
		Debug.Log("Living entity HIT!");
		TakeDamage(damage);
	}

	public virtual void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0 && !dead)
		{
			Die();
		}
	}

	[ContextMenu("Self Destruct")]
	public virtual void Die()
	{
		dead = true;
		if (OnDeath != null)
		{
			OnDeath();
		}
		Destroy(gameObject);
	}
}

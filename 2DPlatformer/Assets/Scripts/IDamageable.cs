using UnityEngine;

public interface IDamageable
{
	void TakeHit(float damage, Vector2 hitPoint, Vector2 hitDirection);

	void TakeDamage(float damage);
}

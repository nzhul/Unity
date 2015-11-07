using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;
	float speed = 10;
	float damage = 1;
	float lifeTime = 3;
	float skinWidth = .1f; // Used to fix collision detection in very close range

	void Start()
	{
		Destroy(gameObject, lifeTime);

		Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
		if (initialCollisions.Length > 0)
		{
			OnHitObject(initialCollisions[0], transform.position);
		}
	}

	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
	}

	void Update () {
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions(moveDistance);
		transform.Translate(Vector3.forward * moveDistance);
	}

	void CheckCollisions(float moveDistance)
	{
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask,QueryTriggerInteraction.Collide))
		{
			OnHitObject(hit.collider, hit.point);
		}
	}

	void OnHitObject(Collider c, Vector3 hitPoint)
	{
		IDamageable damageableObject = c.GetComponent<IDamageable>();
		if (damageableObject != null)
		{
			damageableObject.TakeHit(damage, hitPoint, transform.forward);
		}
		Destroy(gameObject);
	}
}

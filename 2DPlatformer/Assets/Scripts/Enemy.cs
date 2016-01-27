using UnityEngine;
using System.Collections;

public class Enemy : LivingEntity {

	public enum State
	{
		Idle,
		Wander,
		Chasing,
		Attacking
	}

	public State currentState;
	Transform target;
	float gravity = 2f;
	Vector3 velocity;

	Controller2D controller;

	void Start()
	{
		base.Start();
		controller = GetComponent<Controller2D>();
	}

	float direction = -5;

	void Update()
	{
		if (currentState == State.Wander)
		{
			if (controller.collisions.left)
			{
				direction = 5;
			}
			else if (controller.collisions.right)
			{
				direction = -5;
			}

			velocity.x = direction;
		}

		velocity.y -= gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}

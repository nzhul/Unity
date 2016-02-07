using UnityEngine;
using System.Collections;
using System;

public class Enemy : LivingEntity {

	public enum State
	{
		Idle,
		Wander,
		Chasing,
		Attacking
	}

	public State currentState;
	public float enemySpeed = 5;
	public Transform healthBar;
	Transform target;
	float gravity = 2f;
	Vector3 velocity;

	Controller2D controller;

	void Start()
	{
		base.Start();
		controller = GetComponent<Controller2D>();
		enemySpeed = -enemySpeed;
	}

	void Update()
	{
		UpdateHealthBar();

		if (currentState == State.Wander)
		{
			if (controller.collisions.left)
			{
				enemySpeed = Mathf.Abs(enemySpeed);
			}
			else if (controller.collisions.right)
			{
				enemySpeed = -enemySpeed;
			}

			velocity.x = enemySpeed;
		}

		velocity.y -= gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}

	private void UpdateHealthBar()
	{
		float healthPercent = 0;
		healthPercent = health / startingHealth;
		healthBar.localScale = new Vector3(healthPercent, .1f, .1f);
	}
}

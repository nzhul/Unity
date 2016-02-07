using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Player;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(AbilityManager))]
public class Player : MonoBehaviour {

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 6;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;
	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	[HideInInspector]
	public List<GameObject> Enemies;
	GameObject closestEnemy;

	[Header("Shooting:")]
	public float shootingRange = 6f;
	public Projectile projectile;

	[Header("Melee:")]
	public float heroAttackRange = 1f;
	public float meleeDamage = 1f;
	public float attackCd = 1f;
	private float attackTimer = 0;
	private bool IsAttacking = false;

	SpriteRenderer spriteRenderer;
	Animator animator;
	Controller2D controller;
	AbilityManager abilityManager;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		controller = GetComponent<Controller2D>();
		animator = GetComponent<Animator>();
		abilityManager = GetComponent<AbilityManager>();
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2); // This is some physics formula :)
		maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
		controller.OnEnemyCollision += OnEnemyCollision;

		// TODO: Update enemies list on Update method but with Enumerator on every 1-2 seconds
		// TODO: We may not need to update the list on every frame but just to find the new closest enemy.
		// TODO: The update for the list entries must be done via message when new enemy dies or spawns.
		Enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();

		foreach (GameObject enemy in Enemies)
		{
			Enemy enemyComponent = enemy.GetComponent<Enemy>();
			enemyComponent.OnDeath += OnEnemyDeath;
		}

		//Currently not working.Find a way to fix this or use mouse targeting
		StartCoroutine(FindClosestEnemy());
	}

	private void OnEnemyDeath()
	{
		Enemies.Remove(closestEnemy);
	}

	void OnEnemyCollision()
	{
		//Debug.Log("Enemy collision");
	}

	void Update()
	{
		MeleeAttack();

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}

		if (!IsAttacking)
		{
			if (velocity.y != 0)
			{
				animator.Play("NinjaJump");
			}
			else if (input.x != 0)
			{
				if (input.x > 0)
				{
					spriteRenderer.flipX = false;
				}
				else
				{
					spriteRenderer.flipX = true;
				}
				animator.Play("NinjaRun");
			}
			else
			{
				animator.Play("NinjaIdle");
			}
		}

		int wallDirX = (controller.collisions.left) ? -1 : 1;

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		bool wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
		{
			wallSliding = true;
			if (velocity.y < -wallSlideSpeedMax)
			{
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0)
			{
				velocityXSmoothing = 0;
				velocity.x = 0;
				if (input.x != wallDirX && input.x != 0)
				{
					timeToWallUnstick -= Time.deltaTime;
				}
				else
				{
					timeToWallUnstick = wallStickTime;
				}
			}
			else
			{
				timeToWallUnstick = wallStickTime;
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (wallSliding)
			{
				if (abilityManager.Abilities["WallJump"].IsUnlocked)
				{
					if (wallDirX == input.x)
					{
						velocity.x = -wallDirX * wallJumpClimb.x;
						velocity.y = wallJumpClimb.y;
					}
					else if (input.x == 0)
					{
						velocity.x = -wallDirX * wallJumpOff.x;
						velocity.y = wallJumpOff.y;
					}
					else
					{
						velocity.x = -wallDirX * wallLeap.x;
						velocity.y = wallLeap.y;
					}
				}

				if (!abilityManager.Abilities["WallJump"].IsUnlocked)
				{
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				}
			}

			if (controller.collisions.below)
			{
				velocity.y = maxJumpVelocity;
			}
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			if (velocity.y > minJumpVelocity)
			{
				velocity.y = minJumpVelocity;
			}
		}

		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);

		if (controller.collisions.above || controller.collisions.below)
		{
			velocity.y = 0;
		}
	}

	private void MeleeAttack()
	{
		float directionX = controller.collisions.faceDir;
		Vector2 topLeftOrigin = Vector2.zero;
        Vector2 bottomRight = Vector2.zero;

		if (directionX == 1)
		{
			topLeftOrigin = new Vector2(transform.position.x + (controller.bocCollider.size.x / 2), controller.raycastOrigins.topLeft.y);
			bottomRight = new Vector2(transform.position.x + (controller.bocCollider.size.x / 2) + heroAttackRange, controller.raycastOrigins.bottomRight.y);
		}
		else if (directionX == -1)
		{
			topLeftOrigin = new Vector2(transform.position.x - (controller.bocCollider.size.x / 2) - heroAttackRange, controller.raycastOrigins.topLeft.y);
			bottomRight = new Vector2(transform.position.x - (controller.bocCollider.size.x / 2), controller.raycastOrigins.bottomRight.y);
		}

		Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(topLeftOrigin, bottomRight, controller.enemyCollisionMask);

		if (hitEnemies.Length > 0)
		{
			for (int i = 0; i < hitEnemies.Length; i++)
			{
				PerformAttack(hitEnemies[i]);
			}
		}
		else
		{
			IsAttacking = false;
		}
	}

	private void PerformAttack(Collider2D hitTarget)
	{
		IDamageable damageableObject = hitTarget.GetComponent<IDamageable>();
		if (damageableObject != null && !IsAttacking)
		{
			IsAttacking = true;
			attackTimer = attackCd;
			animator.Play("NinjaMeleeAttack");
			damageableObject.TakeHit(meleeDamage, hitTarget.transform.position, hitTarget.transform.position.normalized);
		}

		if (attackTimer > 0)
		{
			attackTimer -= Time.deltaTime;
		}
		else
		{
			IsAttacking = false;
		}
	}

	void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			float directionX = controller.collisions.faceDir;
			float verticalAttackRange = controller.raycastOrigins.topLeft.y - controller.raycastOrigins.bottomLeft.y;
			Gizmos.color = new Color(1, 1, 0, .5f);
			if (directionX == 1)
			{
				Gizmos.DrawCube(new Vector3(controller.raycastOrigins.topRight.x + (heroAttackRange / 2), transform.position.y + controller.bocCollider.offset.y, 0), new Vector3(heroAttackRange, verticalAttackRange, 0));
			}
			else if (directionX == -1)
			{
				Gizmos.DrawCube(new Vector3(controller.raycastOrigins.topLeft.x - (heroAttackRange / 2), transform.position.y + controller.bocCollider.offset.y, 0), new Vector3(heroAttackRange, verticalAttackRange, 0));
			}
		}
	}


	private void Shoot()
	{
		if (closestEnemy == null)
		{
			closestEnemy = Enemies.OrderBy(go => Vector3.Distance(go.transform.position, transform.position)).FirstOrDefault();
		}

		if (closestEnemy != null)
		{
			float distance = Vector2.Distance(transform.position, closestEnemy.transform.position);
			if (distance < shootingRange)
			{
				Projectile newProjetile = Instantiate(projectile, transform.position, transform.rotation) as Projectile;

				Enemy theEnemy = closestEnemy.GetComponent<Enemy>();
				newProjetile.target = theEnemy;
			}
		}
	}

	IEnumerator FindClosestEnemy()
	{
		closestEnemy = Enemies.OrderBy(go => Vector3.Distance(go.transform.position, transform.position)).FirstOrDefault();
		yield return new WaitForSeconds(1);
	}
}

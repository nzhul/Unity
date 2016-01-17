using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 10;
	public int jumpSpeed = 10;
	public float shurikenSpeed = 3;
	Vector3 shurikenOffset = new Vector3(.3f, 0, 0);
	SpriteRenderer renderer;
	Rigidbody2D body;
	Animator animator;
	float playerStartingPositionX;
	float playerStartingPositionY;
	public GameObject shuriken;
	PlayerState state;

	void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
		body = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		playerStartingPositionX = transform.position.x;
		playerStartingPositionY = transform.position.y;
		state = PlayerState.Live;
	}

	void Update()
	{
		animator.SetBool("IsRunning", false);

		if (state == PlayerState.Live)
		{
			if (Input.GetAxis("Horizontal") > 0)
			{
				renderer.flipX = false;
				body.AddForce(new Vector2(1, 0) * speed);
				animator.SetBool("IsRunning", true);
			}

			if (Input.GetAxis("Horizontal") < 0)
			{
				renderer.flipX = true;
				body.AddForce(new Vector2(-1, 0) * speed);
				animator.SetBool("IsRunning", true);
			}

			if (Input.GetKeyDown(KeyCode.Space) && body.velocity.y == 0)
			{
				body.AddForce(new Vector2(0, 1) * jumpSpeed, ForceMode2D.Impulse);
				animator.SetTrigger("IsJumping");
			}

			// Fire
			if (Input.GetKeyDown(KeyCode.R))
			{
				animator.SetTrigger("IsFiring");
				GameObject s = (GameObject)Instantiate(shuriken, transform.position + shurikenOffset, new Quaternion(0,0,0,0));
				Rigidbody2D rb = s.GetComponent<Rigidbody2D>();
				rb.AddForce(Vector2.right * shurikenSpeed, ForceMode2D.Impulse);

			}

		}
	}

	void OnCollisionEnter2D(Collision2D hit)
	{
		if (hit.collider.tag == "Fire")
		{
			animator.SetTrigger("IsDead");
			state = PlayerState.Dead;
			StartCoroutine(RespawnAfterTime(2));
		}
	}

	IEnumerator RespawnAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		animator.SetTrigger("Respawn");
		transform.position = new Vector2(playerStartingPositionX, playerStartingPositionY);
		state = PlayerState.Live;
	}

	enum PlayerState
	{
		Live,
		Dead
	}
}

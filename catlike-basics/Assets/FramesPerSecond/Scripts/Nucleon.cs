using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Nucleon : MonoBehaviour {

	#region Properties

	public float attractionForce;

	#endregion

	#region Fields

	private Rigidbody body;

	#endregion

	private void Awake()
	{
		body = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		body.AddForce(transform.localPosition * -attractionForce);
	}
}

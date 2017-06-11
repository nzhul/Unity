using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Stuff : PooledObject {

	public Rigidbody Body { get; private set; }

	MeshRenderer[] meshRenderers;

	private void Awake()
	{
		Body = GetComponent<Rigidbody>();
		meshRenderers = GetComponentsInChildren<MeshRenderer>();
	}

	public void SetMaterial(Material m)
	{
		for (int i = 0; i < meshRenderers.Length; i++)
		{
			meshRenderers[i].material = m;
		}
	}

	private void OnTriggerEnter(Collider enteredCollider)
	{
		if (enteredCollider.CompareTag("Kill Zone"))
		{
			ReturnToPool();
		}
	}

	private void OnLevelWasLoaded(int level)
	{
		ReturnToPool();
	}
}

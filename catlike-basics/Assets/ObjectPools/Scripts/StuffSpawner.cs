using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffSpawner : MonoBehaviour {

	public FloatRange timeBetweenSpawns, scale, randomVelocity, angularVelocity;
	public Stuff[] stuffPrefabs;
	public float velocity = 15;
	public Material stuffMaterial;

	float timeSinceLastSpawn;
	float currentSpawnDelay;

	private void FixedUpdate()
	{
		timeSinceLastSpawn += Time.deltaTime;
		if (timeSinceLastSpawn >= currentSpawnDelay)
		{
			timeSinceLastSpawn -= currentSpawnDelay;
			currentSpawnDelay = timeBetweenSpawns.RandomInRange;
			SpawnStuff();
		}
	}

	private void SpawnStuff()
	{
		Stuff prefab = stuffPrefabs[Random.Range(0, stuffPrefabs.Length)];
		Stuff spawn = prefab.GetPooledInstance<Stuff>();
		spawn.transform.localPosition = transform.position;
		spawn.transform.localScale = Vector3.one * scale.RandomInRange;
		spawn.transform.localRotation = Random.rotation;
		spawn.Body.velocity = transform.up * velocity + Random.onUnitSphere * randomVelocity.RandomInRange;
		spawn.Body.angularVelocity = Random.onUnitSphere * angularVelocity.RandomInRange;
		spawn.SetMaterial(stuffMaterial);
	}
}

[System.Serializable]
public struct FloatRange
{
	public float min, max;

	public float RandomInRange
	{
		get
		{
			return Random.Range(min, max);
		}
	}
}
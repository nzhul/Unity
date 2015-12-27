using UnityEngine;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

	public static int score { get; private set; }
	float lastEnemyKillTime;
	int streakCount;
	int maxStreakCount = 5;
	float streakExpiryTime = 1;

	void Start()
	{
		Enemy.OnDeathStatic += OnEnemyKilled;
		FindObjectOfType<Player>().OnDeath += OnPlayerDeath;
	}

	void OnEnemyKilled()
	{
		if (Time.time < lastEnemyKillTime + streakExpiryTime && streakCount <= maxStreakCount)
		{
			streakCount++;
		}
		else
		{
			streakCount = 0;
		}

		lastEnemyKillTime = Time.time;
		score += 5 + (int)Mathf.Pow(2, streakCount);
	}

	void OnPlayerDeath()
	{
		score = 0;
		Enemy.OnDeathStatic -= OnEnemyKilled;
	}
}

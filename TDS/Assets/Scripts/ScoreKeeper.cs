using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ScoreKeeper : MonoBehaviour {

	public static int score { get; private set; }
	float lastEnemyKillTime;
	int streakCount;
	int maxStreakCount = 5;
	float streakExpiryTime = 1;
	string currentUser = "unknown";

	public Text debugInfo;

	void Start()
	{
		 Enemy.OnDeathStatic += OnEnemyKilled;
		FindObjectOfType<Player>().OnDeath += OnPlayerDeath;
		StartCoroutine(GetCurrentUser());
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
		this.AddNewHighscore(currentUser, score);
		score = 0;
		Enemy.OnDeathStatic -= OnEnemyKilled;
	}

	IEnumerator UploadNewHighscore(string username, int score)
	{
		var postUrl = "http://www.d3bg.org/telerikacademy/unity/TDS/scoreservice/index.php";
		var form = new WWWForm();
		form.AddField("score", score.ToString());
		form.AddField("username", username);
		form.AddField("gamepassword", "simplePassword1!");

		var www = new WWW(postUrl, form);

		yield return www;

		if (www.error == null)
		{
			//Debug.Log("Success: " + www.text);
			//debugInfo.text = www.text;
		}
		else
		{
			Debug.Log("Error: " + www.error);
			debugInfo.text = www.error;
		}
	}

	public void AddNewHighscore(string username, int score)
	{
		StartCoroutine(UploadNewHighscore(username, score));
	}

	IEnumerator GetCurrentUser()
	{
		string url = "http://www.d3bg.org/telerikacademy/unity/TDS/scoreservice/currentuser.php";
		WWW www = new WWW(url);

		yield return www;

		if (www.error == null)
		{
			Debug.Log("User: " + www.text);
			if (www.text != "unknown")
			{
				debugInfo.text = "Hello, " + www.text;
				currentUser = www.text;
			}
			else
			{
				currentUser = "unknown";
			}

		}
		else
		{
			Debug.Log("Error: " + www.error);
			debugInfo.text = www.error;
		}
	}
}

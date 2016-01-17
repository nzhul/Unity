using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	Text timeLabel;

	void Start() {
		timeLabel = GetComponent<Text>();
	}

	void Update()
	{
		timeLabel.text = "Time: " + Time.time.ToString();
	}
}

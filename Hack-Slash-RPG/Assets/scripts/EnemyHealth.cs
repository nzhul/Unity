using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	public int maxHealth = 100;
	public int currentHealth = 100;
	public float healthBarLength;

	// Use this for initialization
	void Start () {
		healthBarLength = Screen.width / 2;
	}
	
	// Update is called once per frame
	void Update () {
		AdjustCurrentHealth(0);
	}
	
	void OnGUI()
	{
		GUI.Box(new Rect(10, 40, healthBarLength, 20),currentHealth + "/" + maxHealth);
	}
	
	public void AdjustCurrentHealth(int adj)
	{
		currentHealth += adj;
		if (currentHealth < 0) {
			currentHealth = 0;
		}
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
		if (maxHealth < 1) {
			maxHealth = 1;
		}
		healthBarLength = (Screen.width / 2) * (currentHealth / (float)maxHealth);
	}
}

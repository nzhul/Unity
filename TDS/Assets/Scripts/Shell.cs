using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour {

	public Rigidbody myRigitbody;
	public float forceMin;
	public float forceMax;

	public float lifeTime = 4;
	public float fadeTime = 2;

	void Start () {
		float force = Random.Range(forceMin, forceMax);

		myRigitbody.AddForce(transform.right * force);
		myRigitbody.AddTorque(Random.insideUnitSphere * force);

		StartCoroutine(Fade());
	}
	
	void Update () {
	
	}

	IEnumerator Fade()
	{
		yield return new WaitForSeconds(lifeTime);

		float percent = 0;
		float fadeSpeed = 1 / fadeTime;

		Material mat = GetComponent<Renderer>().material;
		Color initialColor = mat.color;

		while (percent < 1)
		{
			percent += Time.deltaTime * fadeSpeed;
			mat.color = Color.Lerp(initialColor, Color.clear, percent);
			yield return null;
		}

		Destroy(gameObject);
	}
}
using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hitInfo;

		if (Physics.Raycast(ray, out hitInfo))
		{
			Debug.Log("Raycast hit: " + hitInfo.collider.transform.parent.name);

			GameObject ourHitObject = hitInfo.collider.transform.gameObject;

			if (Input.GetMouseButtonDown(0))
			{
				MeshRenderer mr = ourHitObject.GetComponent<MeshRenderer>();

				if (mr.material.color == Color.red)
				{
				mr.material.color = Color.white;
				}
				else
				{
					mr.material.color = Color.red;
				}
			}
		}
	}
}

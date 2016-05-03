using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public GameObject hexPrefab;

	private int height = 20;
	private int width = 20;

	float xOffset = 0.882f;
	float zOffset = 0.764f;

	void Start () {
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				float xPos = x * xOffset;

				if (y % 2 == 0)
				{
					xPos += xOffset / 2f;
				}

				GameObject hex_go = Instantiate(hexPrefab, new Vector3(xPos * 2, 0, y * zOffset * 2), Quaternion.identity) as GameObject;
				hex_go.name = "Hex_" + x + "_" + y;

				hex_go.transform.SetParent(this.transform);

				hex_go.isStatic = true;
			}
		}
	}

}

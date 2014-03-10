using UnityEngine;
using System.Collections;

public class WingControler : MonoBehaviour {

    public Transform backWing;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("w"))
        {
            backWing.Rotate(Vector3.right * Time.deltaTime * 20.0f);
        }
        if (Input.GetKey("s"))
        {
            backWing.Rotate(-Vector3.right * Time.deltaTime * 20.0f);
        }
	}
}

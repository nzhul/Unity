using UnityEngine;
using System.Collections;

public class CarCamController : MonoBehaviour {
    public Camera useCamera;
    public Transform trackObject;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        useCamera.transform.LookAt(trackObject);
	}
}

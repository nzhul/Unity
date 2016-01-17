using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

    public float speed = 10f;
    public Vector3 rotation = new Vector3(0, 0, 1);

    void Update () {
        transform.Rotate(rotation, speed * Time.deltaTime);
    }
}

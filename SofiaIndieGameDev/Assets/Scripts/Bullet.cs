using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    void Start()
    {
        var trail = GetComponentInChildren<TrailRenderer>();
        trail.sortingLayerName = "Players";
        trail.sortingOrder = 2;
    }

    void OnTriggerEnter2D(Collider2D col){
        Destroy(gameObject);
    }
}

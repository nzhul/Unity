using UnityEngine;
using System.Collections;

public class ResetFromCrash : MonoBehaviour {

    void OnCollisionEnter(Collision theCollision)
    {
        CarController otherObjectsScript = theCollision.gameObject.GetComponent<CarController>();
        if (otherObjectsScript != null)
        {
            otherObjectsScript.Respawn();
        }
    }
}

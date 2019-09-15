using UnityEngine;

namespace Assets.Scripts
{
    public class TargetPoint : MonoBehaviour
    {
        public Enemy Enemy { get; set; }

        public Vector3 Position => transform.position;

        private void Awake()
        {
            Enemy = transform.root.GetComponent<Enemy>();
            Debug.Assert(Enemy != null, "Target point without Enemy root!", this);
            Debug.Assert(gameObject.layer == 9, "Target point on wrong layer!", this);
        }
    }
}

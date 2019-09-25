using UnityEngine;

namespace Assets.Scripts
{
    public class TargetPoint : MonoBehaviour
    {

        const int enemyLayerMask = 1 << 9;

        static Collider[] buffer = new Collider[100];

        public Enemy Enemy { get; set; }

        public Vector3 Position => transform.position;

        public static int BufferedCount { get; private set; }

        public static TargetPoint RandomBuffered => GetBuffered(Random.Range(0, BufferedCount));

        private void Awake()
        {
            Enemy = transform.root.GetComponent<Enemy>();
            Debug.Assert(Enemy != null, "Target point without Enemy root!", this);
            Debug.Assert(gameObject.layer == 9, "Target point on wrong layer!", this);
            this.Enemy.TargetPointCollider = GetComponent<Collider>();
        }

        public static bool FillBuffer(Vector3 position, float range)
        {
            BufferedCount = Physics.OverlapSphereNonAlloc(position, range, buffer, enemyLayerMask);
            return BufferedCount > 0;
        }

        public static TargetPoint GetBuffered(int index)
        {
            var target = buffer[index].GetComponent<TargetPoint>();
            Debug.Assert(target != null, "Targeted non-enemy!", buffer[0]);
            return target;
        }
    }
}

using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Tower : GameTileContent
    {

        [SerializeField, Range(1.5f, 10.5f)]
        protected float targetingRange = 1.5f;

        public abstract TowerType TowerType { get; }

        protected bool TrackTarget(ref TargetPoint target)
        {
            if (target == null || !target.Enemy.IsValidTarget)
            {
                return false;
            }

            Vector3 a = transform.localPosition;
            Vector3 b = target.Position;

            if (Vector3.Distance(a, b) > targetingRange + 0.125f * target.Enemy.Scale)
            {
                target = null;
                return false;
            }

            return true;
        }

        protected bool AcquireTarget(out TargetPoint target)
        {
            // For future reference:
            //int hits = Physics.OverlapSphereNonAlloc(transform.localPosition, targetingRange, targetsBuffer, enemyLayerMask);

            //if (hits > 0)
            //{
            //    target = targetsBuffer[UnityEngine.Random.Range(0, hits)].GetComponent<TargetPoint>();
            //    Debug.Assert(target != null, "Targeted non-enemy!", targetsBuffer[0]);
            //    return true;
            //}

            if (TargetPoint.FillBuffer(transform.localPosition, targetingRange))
            {
                target = TargetPoint.RandomBuffered;
                return true;
            }

            target = null;
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 position = transform.localPosition;
            position.y += 0.01f;
            Gizmos.DrawWireSphere(position, targetingRange);
        }
    }
}

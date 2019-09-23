using UnityEngine;

namespace Assets.Scripts
{
    public class Shell : WarEntity
    {
        Vector3 launchPoint, targetPoint, launchVelocity;
        float age, blastRadius, damage;

        public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity,
            float blastRadius, float damage)
        {
            this.launchPoint = launchPoint;
            this.targetPoint = targetPoint;
            this.launchVelocity = launchVelocity;
            this.blastRadius = blastRadius;
            this.damage = damage;
        }

        public override bool GameUpdate()
        {
            age += Time.deltaTime;
            Vector3 p = launchPoint + launchVelocity * age;
            p.y -= 0.5f * 9.81f * age * age;
            transform.localPosition = p;

            if (p.y <= 0f)
            {
                Game.SpawnExplosion().Initialize(targetPoint, blastRadius, damage);
                OriginFactory.Reclaim(this);
                return false;
            }

            Vector3 d = launchVelocity;
            d.y -= 9.81f * age;
            transform.localRotation = Quaternion.LookRotation(d);
            Game.SpawnExplosion().Initialize(p, 0.1f);
            return true;
        }
    }
}

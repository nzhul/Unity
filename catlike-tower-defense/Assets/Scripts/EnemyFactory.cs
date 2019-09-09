using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu]
    public class EnemyFactory : GameObjectFactory
    {
        [SerializeField]
        Enemy prefab = default;

        [SerializeField, FloatRangeSlider(0.5f, 2f)]
        FloatRange scale = new FloatRange(1f);

        public Enemy Get()
        {
            Enemy instance = CreateGameObjectInstance(prefab);
            instance.OriginFactory = this;
            instance.Initialize(scale.RandomValueInRange);
            return instance;
        }

        public void Reclaim(Enemy enemy)
        {
            Debug.Assert(enemy.OriginFactory == this, "Wrong factory reclaimed!");
            Destroy(enemy.gameObject);
        }
    }
}

using System;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class EnemySpawnSequence
    {
        [SerializeField]
        EnemyFactory factory = default;

        [SerializeField]
        EnemyType type = EnemyType.Medium;

        [SerializeField, Range(1, 100)]
        int amount = 1;

        [SerializeField, Range(0.1f, 10f)]
        float cooldown = 1f;

        public State Begin() => new State(this);

        [Serializable]
        public struct State
        {
            EnemySpawnSequence sequence;

            int count;

            float cooldown;

            public State(EnemySpawnSequence sequence)
            {
                this.sequence = sequence;
                count = 0;
                cooldown = sequence.cooldown;
            }

            public float Progress(float deltaTime)
            {
                cooldown += deltaTime;
                while (cooldown >= sequence.cooldown)
                {
                    cooldown -= sequence.cooldown;
                    if (count >= sequence.amount)
                    {
                        return cooldown;
                    }

                    count += 1;
                    Game.SpawnEnemy(sequence.factory, sequence.type);
                }

                return -1f;
            }
        }
    }
}
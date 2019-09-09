using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class FloatRange
    {
        [SerializeField]
        float min, max;

        public float Min => min;

        public float Max => max;

        public float RandomValueInRange
        {
            get
            {
                return Random.Range(min, max);
            }
        }

        public FloatRange(float value)
        {
            min = max = value;
        }

        public FloatRange(float min, float max)
        {
            this.min = min;
            this.max = max < min ? min : max;
        }
    }
}
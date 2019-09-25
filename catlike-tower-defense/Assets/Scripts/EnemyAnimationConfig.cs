using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu]
    public class EnemyAnimationConfig : ScriptableObject
    {
        [SerializeField]
        AnimationClip move = default, intro = default, outro = default, dying = default;

        [SerializeField]
        AnimationClip appear = default, disappear = default;

        [SerializeField]
        float moveAnimationSpeed = 1f;

        bool hasAppearClip, hasDisappearClip;

        public AnimationClip Move => move;

        public AnimationClip Intro => intro;

        public AnimationClip Outro => outro;

        public AnimationClip Dying => dying;

        public AnimationClip Appear => appear;

        public AnimationClip Disappear => disappear;

        public float MoveAnimationSpeed => moveAnimationSpeed;
    }
}
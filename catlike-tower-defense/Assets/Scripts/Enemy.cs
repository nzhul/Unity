using UnityEngine;
using static Assets.Scripts.EnemyAnimator;

namespace Assets.Scripts
{
    public class Enemy : GameBehavior
    {
        [SerializeField]
        Transform model = default;

        [SerializeField]
        EnemyAnimationConfig animationConfig = default;

        EnemyFactory originFactory;

        GameTile tileFrom, tileTo;
        Vector3 positionFrom, positionTo;
        float progress, progressFactor;

        Direction direction;
        DirectionChange directionChange;
        float directionAngleFrom, directionAngleTo;

        float pathOffset;

        float speed;

        Collider targetPointCollider;

        EnemyAnimator animator;

        public bool IsValidTarget => animator.CurrentClip == Clip.Move;

        public Collider TargetPointCollider
        {
            set
            {
                Debug.Assert(targetPointCollider == null, "Redefined collider!");
                targetPointCollider = value;
            }
        }

        public float Health { get; set; }

        public float Scale { get; private set; }

        public EnemyFactory OriginFactory
        {
            get => originFactory;
            set
            {
                Debug.Assert(originFactory == null, "Redefined origin factory!");
                originFactory = value;
            }
        }

        private void Awake()
        {
            animator.Configure(
                model.GetChild(0).gameObject.AddComponent<Animator>(),
                animationConfig
                );
        }

        public void SpawnOn(GameTile tile)
        {
            Debug.Assert(tile.NextTileOnPath != null, "Nowhere to go!", this);
            tileFrom = tile;
            tileTo = tile.NextTileOnPath;
            transform.localRotation = tileFrom.PathDirection.GetRotation();
            progress = 0f;
            PrepareIntro();
        }

        public void Initialize(float scale, float speed, float pathOffset, float health)
        {
            Scale = scale;
            model.localScale = new Vector3(scale, scale, scale);
            this.speed = speed;
            this.pathOffset = pathOffset;
            this.Health = health;
            animator.PlayIntro();
            targetPointCollider.enabled = false;
        }

        public void ApplyDamage(float damage)
        {
            Debug.Assert(damage >= 0f, "Negative damage applied.");
            this.Health -= damage;
        }

        public override bool GameUpdate()
        {
            animator.GameUpdate();

            if (animator.CurrentClip == Clip.Intro)
            {
                if (!animator.IsDone)
                {
                    return true;
                }

                animator.PlayMove(speed / Scale);
                targetPointCollider.enabled = true;
            }
            else if (animator.CurrentClip >= Clip.Outro)
            {
                if (animator.IsDone)
                {
                    Recycle();
                    return false;
                }

                return true;
            }

            if (this.Health <= 0f)
            {
                animator.PlayDying();
                targetPointCollider.enabled = false;
                return true;
            }

            progress += Time.deltaTime * progressFactor;
            while (progress >= 1f)
            {
                if (tileTo == null)
                {
                    Game.EnemyReachedDestination();
                    animator.PlayOutro();
                    targetPointCollider.enabled = false;
                    return true;
                }

                progress = (progress - 1f) / progressFactor;
                PrepareNextState();
                progress *= progressFactor;
            }

            if (directionChange == DirectionChange.None)
            {
                transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);
            }
            else
            {
                float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, progress);
                transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            }
            return true;
        }

        public override void Recycle()
        {
            animator.Stop();
            OriginFactory.Reclaim(this);
        }

        void PrepareForward()
        {
            transform.localRotation = direction.GetRotation();
            directionAngleTo = direction.GetAngle();
            model.localPosition = new Vector3(pathOffset, 0f);
            progressFactor = speed;
        }

        void PrepareTurnRight()
        {
            directionAngleTo = directionAngleFrom + 90f;
            model.localPosition = new Vector3(pathOffset - 0.5f, 0f);
            transform.localPosition = positionFrom + direction.GetHalfVector();
            progressFactor = speed / (Mathf.PI * 0.5f * (0.5f - pathOffset));
        }

        void PrepareTurnLeft()
        {
            directionAngleTo = directionAngleFrom - 90f;
            model.localPosition = new Vector3(pathOffset + 0.5f, 0f);
            transform.localPosition = positionFrom + direction.GetHalfVector();
            progressFactor = speed / (Mathf.PI * 0.5f * (0.5f + pathOffset));
        }

        void PrepareTurnAround()
        {
            directionAngleTo = directionAngleFrom + (pathOffset < 0f ? 180f : -180f);
            model.localPosition = new Vector3(pathOffset, 0f);
            transform.localPosition = positionFrom;
            progressFactor = speed / (Mathf.PI * Mathf.Max(Mathf.Abs(pathOffset), 0.2f));
        }

        void PrepareNextState()
        {
            tileFrom = tileTo;
            tileTo = tileTo.NextTileOnPath;
            positionFrom = positionTo;
            if (tileTo == null)
            {
                PrepareOutro();
                return;
            }
            positionTo = tileFrom.ExitPoint;
            directionChange = direction.GetDirectionChangeTo(tileFrom.PathDirection);
            direction = tileFrom.PathDirection;
            directionAngleFrom = directionAngleTo;

            switch (directionChange)
            {
                case DirectionChange.None: PrepareForward(); break;
                case DirectionChange.TurnRight: PrepareTurnRight(); break;
                case DirectionChange.TurnLeft: PrepareTurnLeft(); break;
                default: PrepareTurnAround(); break;
            }
        }

        void PrepareIntro()
        {
            positionFrom = tileFrom.transform.localPosition;
            transform.localPosition = positionFrom;
            positionTo = tileFrom.ExitPoint;
            direction = tileFrom.PathDirection;
            directionChange = DirectionChange.None;
            directionAngleFrom = directionAngleTo = direction.GetAngle();
            model.localPosition = new Vector3(pathOffset, 0f);
            transform.localRotation = tileFrom.PathDirection.GetRotation();
            progressFactor = 2f * speed;
        }

        void PrepareOutro()
        {
            positionTo = tileFrom.transform.localPosition;
            directionChange = DirectionChange.None;
            directionAngleTo = direction.GetAngle();
            model.localPosition = new Vector3(pathOffset, 0f);
            transform.localRotation = direction.GetRotation();
            progressFactor = 2f * speed;
        }

        private void OnDestroy()
        {
            animator.Destroy();
        }
    }
}
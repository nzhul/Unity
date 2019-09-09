using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        Transform model = default;

        EnemyFactory originFactory;

        GameTile tileFrom, tileTo;
        Vector3 positionFrom, positionTo;
        float progress, progressFactor;

        Direction direction;
        DirectionChange directionChange;
        float directionAngleFrom, directionAngleTo;

        public EnemyFactory OriginFactory
        {
            get => originFactory;
            set
            {
                Debug.Assert(originFactory == null, "Redefined origin factory!");
                originFactory = value;
            }
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

        public void Initialize(float scale)
        {
            model.localScale = new Vector3(scale, scale, scale);
        }

        void PrepareForward()
        {
            transform.localRotation = direction.GetRotation();
            directionAngleTo = direction.GetAngle();
            model.localPosition = Vector3.zero;
            progressFactor = 1f;
        }

        void PrepareTurnRight()
        {
            directionAngleTo = directionAngleFrom + 90f;
            model.localPosition = new Vector3(-0.5f, 0f);
            transform.localPosition = positionFrom + direction.GetHalfVector();
            progressFactor = 1f / (Mathf.PI * 0.25f);
        }

        void PrepareTurnLeft()
        {
            directionAngleTo = directionAngleFrom - 90f;
            model.localPosition = new Vector3(0.5f, 0f);
            transform.localPosition = positionFrom + direction.GetHalfVector();
            progressFactor = 1f / (Mathf.PI * 0.25f);
        }

        void PrepareTurnAround()
        {
            directionAngleTo = directionAngleFrom + 180f;
            model.localPosition = Vector3.zero;
            transform.localPosition = positionFrom;
            progressFactor = 2f;
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
            positionTo = tileFrom.ExitPoint;
            direction = tileFrom.PathDirection;
            directionChange = DirectionChange.None;
            directionAngleFrom = directionAngleTo = direction.GetAngle();
            transform.localRotation = tileFrom.PathDirection.GetRotation();
            progressFactor = 2f;
        }

        void PrepareOutro()
        {
            positionTo = tileFrom.transform.localPosition;
            directionChange = DirectionChange.None;
            directionAngleTo = direction.GetAngle();
            model.localPosition = Vector3.zero;
            transform.localRotation = direction.GetRotation();
            progressFactor = 2f;
        }

        public bool GameUpdate()
        {
            progress += Time.deltaTime * progressFactor;
            while (progress >= 1f)
            {
                if (tileTo == null)
                {
                    OriginFactory.Reclaim(this);
                    return false;
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
    }
}
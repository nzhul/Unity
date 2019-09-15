using UnityEngine;

namespace Assets.Scripts
{
    [SelectionBase]
    public class GameTileContent : MonoBehaviour
    {
        [SerializeField]
        GameTileContentType type = default;

        public GameTileContentType Type => type;

        GameTileContentFactory originFactory;

        public bool BlocksPath => Type == GameTileContentType.Wall || Type == GameTileContentType.Tower;

        public GameTileContentFactory OriginFactory
        {
            get => originFactory;
            set
            {
                Debug.Assert(originFactory == null, "Redefined origin factory!");
                originFactory = value;
            }
        }

        public virtual void GameUpdate()
        {

        }

        public void Recycle()
        {
            originFactory.Reclaim(this);
        }
    }
}

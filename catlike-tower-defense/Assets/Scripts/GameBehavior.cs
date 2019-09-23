using UnityEngine;

namespace Assets.Scripts
{
    public abstract class GameBehavior : MonoBehaviour
    {
        public virtual bool GameUpdate() => true;
    }
}
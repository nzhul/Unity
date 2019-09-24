using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    [Serializable]
    public class GameBehaviourCollection
    {
        List<GameBehavior> behaviors = new List<GameBehavior>();

        public bool IsEmpty => behaviors.Count == 0;

        public void Add(GameBehavior behaviour)
        {
            behaviors.Add(behaviour);
        }

        public void GameUpdate()
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                if (!behaviors[i].GameUpdate())
                {
                    int lastIndex = behaviors.Count - 1;
                    behaviors[i] = behaviors[lastIndex];
                    behaviors.RemoveAt(lastIndex);
                    i -= 1;
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < behaviors.Count; i++)
            {
                behaviors[i].Recycle();
            }
            behaviors.Clear();
        }
    }
}

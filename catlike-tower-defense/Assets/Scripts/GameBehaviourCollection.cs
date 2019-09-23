using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    [Serializable]
    public class GameBehaviourCollection
    {
        List<GameBehavior> behaviours = new List<GameBehavior>();

        public void Add(GameBehavior behaviour)
        {
            behaviours.Add(behaviour);
        }

        public void GameUpdate()
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (!behaviours[i].GameUpdate())
                {
                    int lastIndex = behaviours.Count - 1;
                    behaviours[i] = behaviours[lastIndex];
                    behaviours.RemoveAt(lastIndex);
                    i -= 1;
                }
            }
        }
    }
}

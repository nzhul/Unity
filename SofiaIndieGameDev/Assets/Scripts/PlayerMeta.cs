using UnityEngine;
using System.Collections;

public class PlayerMeta : MonoBehaviour {

    public string playerName;
    public PlayerID playerID;
    private int score = 0;

    public void IncrementScore(int increment = 1)
    {
        score += System.Math.Abs(increment);
    }

    public int GetScore()
    {
        return score;
    }
}

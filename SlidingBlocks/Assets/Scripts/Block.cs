using System;
using UnityEngine;

public class Block : MonoBehaviour {

    public event Action<Block> OnBlockPressed;

    private void OnMouseDown()
    {
        if (OnBlockPressed != null)
        {
            OnBlockPressed(this);
        }
    }
}
using System;
using UnityEngine;

public class Block : MonoBehaviour
{

    public event Action<Block> OnBlockPressed;

    public Vector2Int coord;

    public void Init(Vector2Int startingCoord, Texture2D image)
    {
        this.coord = startingCoord;
        GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
        GetComponent<MeshRenderer>().material.mainTexture = image;
    }

    private void OnMouseDown()
    {
        if (OnBlockPressed != null)
        {
            OnBlockPressed(this);
        }
    }
}
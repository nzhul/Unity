using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public Texture2D image;
    public int blocksPerLine = 4;
    public float border = .5f;
    Block emptyBlock;

    // Use this for initialization
    void Start()
    {
        this.CreateGrid();
    }

    private void CreateGrid()
    {
        Texture2D[,] imageSlices = ImageSlicer.GetSlices(image, blocksPerLine);

        for (int y = 0; y < blocksPerLine; y++)
        {
            for (int x = 0; x < blocksPerLine; x++)
            {
                GameObject blockObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                blockObject.transform.position = -Vector2.one * (blocksPerLine - 1) * .5f + new Vector2(x, y);
                blockObject.transform.parent = transform;

                Block block = blockObject.AddComponent<Block>();
                block.OnBlockPressed += PlayerMoveBlockInput;
                block.Init(new Vector2Int(x, y), imageSlices[x, y]);

                if (y == 0 && x == blocksPerLine - 1)
                {
                    blockObject.SetActive(false);
                    emptyBlock = block;
                }
            }
        }

        Camera.main.orthographicSize = (blocksPerLine / 2) + border;
    }

    private void PlayerMoveBlockInput(Block blockToMove)
    {
        if ((blockToMove.coord - emptyBlock.coord).sqrMagnitude == 1)
        {
            Vector2Int targetCoord = emptyBlock.coord;
            emptyBlock.coord = blockToMove.coord;
            blockToMove.coord = targetCoord;

            Vector2 blockToMovePosition = blockToMove.transform.position;
            blockToMove.transform.position = emptyBlock.transform.position;
            emptyBlock.transform.position = blockToMovePosition;
        }
    }
}

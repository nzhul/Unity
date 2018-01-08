using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{

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
        for (int y = 0; y < blocksPerLine; y++)
        {
            for (int x = 0; x < blocksPerLine; x++)
            {
                GameObject blockObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                blockObject.transform.position = -Vector2.one * (blocksPerLine - 1) * .5f + new Vector2(x, y);
                blockObject.transform.parent = transform;

                Block block = blockObject.AddComponent<Block>();
                block.OnBlockPressed += PlayerMoveBlockInput;

                if (y == 0 && x == blocksPerLine -1)
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
        if ((blockToMove.transform.position - emptyBlock.transform.position).sqrMagnitude == 1)
        {
            Vector2 blockToMovePosition = blockToMove.transform.position;
            blockToMove.transform.position = emptyBlock.transform.position;
            emptyBlock.transform.position = blockToMovePosition;
        }
    }
}

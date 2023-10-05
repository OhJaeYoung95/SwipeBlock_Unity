using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private float blockSize = 0.18f;

    public BlockState type = BlockState.None;

    public int X { get; set; }
    public int Y { get; set; }
    public bool IsMerged { get; set; } = false;

    public void SetIndex(int y, int x)
    {
        Y = y;
        X = x;
    }

    private void OnEnable()
    {
        IsMerged = false;
    }

    //private void MergeBlocks()
    //{
    //    List<Block> connectedBlocks = new List<Block>();
    //    FindConnectedBlocks(this, connectedBlocks);

    //    // 점수 구현
    //}

    //private void FindConnectedBlocks(Block currentBlock, List<Block> connectedBlocks)
    //{
    //    Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    //    foreach (Vector2 direction in directions)
    //    {
    //        RaycastHit2D hit = Physics2D.Raycast(currentBlock.transform.position, direction, blockSize);

    //    }

    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockState
{
    None,
    Spade,
    Diamond,
    Heart,
    Clover,
    Obstcle,
    Count
}

public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance { get; private set; }

    private int[,] blockIndexs = new int[5, 5];
    private Vector2[,] indexPos = new Vector2[5, 5];
    private Block[,] blocks = new Block[5, 5];

    [SerializeField]
    private int initCount = 5;
    [SerializeField]
    private float posOffset = 0.18f;
    private bool isChainMerge = false;

    [SerializeField]
    private Block spadePrefab;
    [SerializeField]
    private Block diamondPrefab;
    [SerializeField]
    private Block heartPrefab;
    [SerializeField]
    private Block cloverPrefab;
    [SerializeField]
    private Block obstaclePrefab;
    [SerializeField]
    private GameObject gridPrefab;

    private List<string> poolKeys = new List<string>();

    private string spadeBlockPoolKey = "SpadeBlockPool";
    private string diamondBlockPoolKey = "DiamondBlockPool";
    private string heartBlockPoolKey = "HeartBlockPool";
    private string cloverBlockPoolKey = "CloverBlockPool";
    private string obstacleBlockPoolKey = "ObstacleBlockPool";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);

        poolKeys.Add(spadeBlockPoolKey);
        poolKeys.Add(diamondBlockPoolKey);
        poolKeys.Add(heartBlockPoolKey);
        poolKeys.Add(cloverBlockPoolKey);
        poolKeys.Add(obstacleBlockPoolKey);
    }

    // Start is called before the first frame update
    private void Start()
    {
        GameObject panel = GameObject.FindGameObjectWithTag("GridPanel");
        for (int y = 0; y < blockIndexs.GetLength(0); ++y)
        {
            for (int x = 0; x < blockIndexs.GetLength(1); ++x)
            {
                blockIndexs[y, x] = 0;
                indexPos[y, x] = new Vector2(-(posOffset * 2) + posOffset * x, -(posOffset * 2) + posOffset * y) * 8;
                GameObject grid = Instantiate(gridPrefab, panel.transform);
                grid.transform.position = indexPos[y, x];
                grid.name = $"{y} x {x}";
            }
        }
        InitRandomCreate();
    }

    private void InitRandomCreate()
    {
        for (int i = 0; i < initCount; i++)
        {
            RandomCreate();
        }
    }
    private void RandomCreate()
    {
        int y = 0;
        int x = 0;
        do
        {
            y = Random.Range(0, blockIndexs.GetLength(0));
            x = Random.Range(0, blockIndexs.GetLength(1));
        } while (blockIndexs[y, x] != 0);


        int ranBlockState = Random.Range((int)BlockState.None + 1, (int)BlockState.Count);
        blockIndexs[y, x] = ranBlockState;
        Block newBlock = ObjectPoolManager.Instance.GetObjectPool<Block>(poolKeys[ranBlockState - 1]);
        //Block newBlock = ObjectPoolManager.Instance.GetObjectPool<Block>(poolKeys[1]);

        // 줄일 수 있는 방법이 있어보임 추후 생각하기
        //switch ((BlockState)ranBlockState)
        //{
        //    case BlockState.Spade:
        //         newBlock = ObjectPoolManager.Instance.GetObjectPool<Block>(spadeBlockPoolKey);
        //        break;
        //    case BlockState.Diamond:
        //        newBlock = ObjectPoolManager.Instance.GetObjectPool<Block>(diamondBlockPoolKey);
        //        break;
        //    case BlockState.Heart:
        //        newBlock = ObjectPoolManager.Instance.GetObjectPool<Block>(heartBlockPoolKey);
        //        break;
        //    case BlockState.Clover:
        //        newBlock = ObjectPoolManager.Instance.GetObjectPool<Block>(cloverBlockPoolKey);
        //        break;
        //    case BlockState.Obstcle:
        //        newBlock = ObjectPoolManager.Instance.GetObjectPool<Block>(obstacleBlockPoolKey);
        //        break;
        //    //default:
        //    //    newBlock = ObjectPoolManager.Instance.GetObjectPool<Block>(cloverBlockPoolKey);
        //    //    break;
        //}
        newBlock.transform.position = indexPos[y, x];
        blocks[y, x] = newBlock;
        blocks[y, x].SetIndex(y, x);
    }
    public void MoveBlocks(float swipeAngle)
    {
        do
        {
            isChainMerge = false;
            if (Mathf.Abs(swipeAngle) < 45)     // Right
            {
                Debug.Log("RightSwipe");
                for (int y = 0; y < blockIndexs.GetLength(0); ++y)
                {
                    for (int x = blockIndexs.GetLength(1) - 1; x >= 0; --x)
                    {
                        if (blockIndexs[y, x] == 0)
                            continue;

                        // 이동 위치 계산
                        Vector2Int moveIndex = IsRightBlockEmpty(y, x);
                        if (moveIndex.x != y || moveIndex.y != x)
                        {
                            MoveBlock(y, x, moveIndex);
                        }
                    }
                }
            }
            else if (Mathf.Abs(swipeAngle) > 135)       // Left
            {
                Debug.Log("LeftSwipe");
                for (int y = 0; y < blockIndexs.GetLength(0); ++y)
                {
                    for (int x = 0; x < blockIndexs.GetLength(1); ++x)
                    {
                        if (blockIndexs[y, x] == 0)
                            continue;

                        Vector2Int moveIndex = IsLeftBlockEmpty(y, x);
                        if (moveIndex.x != y || moveIndex.y != x)
                        {
                            MoveBlock(y, x, moveIndex);
                        }
                    }
                }
            }
            else if (swipeAngle < -45 && swipeAngle > -135)     // Down
            {
                Debug.Log("DownSwipe");
                for (int y = 0; y < blockIndexs.GetLength(0); ++y)
                {
                    for (int x = 0; x < blockIndexs.GetLength(1); ++x)
                    {
                        if (blockIndexs[y, x] == 0)
                            continue;

                        Vector2Int moveIndex = IsDownBlockEmpty(y, x);
                        if (moveIndex.x != y || moveIndex.y != x)
                        {
                            MoveBlock(y, x, moveIndex);
                        }
                    }
                }
            }
            else        // Up
            {
                Debug.Log("UpSwipe");
                for (int y = blockIndexs.GetLength(0) - 1; y >= 0; --y)
                {
                    for (int x = 0; x < blockIndexs.GetLength(1); ++x)
                    {
                        if (blockIndexs[y, x] == 0)
                            continue;
                        Vector2Int moveIndex = IsUpBlockEmpty(y, x);
                        if (moveIndex.x != y || moveIndex.y != x)
                        {
                            MoveBlock(y, x, moveIndex);
                        }
                    }
                }
            }
            MergeBlocks();
            ResetIsMerged();
        } while (isChainMerge);
    }

    public void MoveBlock(int y, int x, Vector2Int moveIndex)
    {
        blocks[y, x].transform.position = indexPos[moveIndex.x, moveIndex.y];
        blocks[moveIndex.x, moveIndex.y] = blocks[y, x];
        blocks[moveIndex.x, moveIndex.y].SetIndex(moveIndex.x, moveIndex.y);
        blocks[y, x] = null;
    }
    public void MergeBlocks()
    {
        List<Block> connectedBlocks = new List<Block>();

        for (int y = 0; y < blockIndexs.GetLength(0); y++)
        {
            for (int x = 0; x < blockIndexs.GetLength(1); x++)
            {
                if (!blocks[y, x])
                    continue;
                if (blocks[y, x].type == BlockState.None || blocks[y, x].type == BlockState.Obstcle || blocks[y, x].IsMerged)
                    continue;

                blocks[y, x].IsMerged = true;
                connectedBlocks.Add(blocks[y, x]);
                FindConnectedBlocks(blocks[y, x], connectedBlocks);

                if (connectedBlocks.Count >= 2)
                {
                    isChainMerge = true;
                    foreach (Block block in connectedBlocks)
                    {
                        blockIndexs[block.Y, block.X] = 0;
                        ObjectPoolManager.Instance.ReturnObjectPool<Block>(poolKeys[(int)blocks[block.Y, block.X].type - 1], block);
                        blocks[block.Y, block.X] = null;
                    }
                }
                connectedBlocks.Clear();
            }
        }
    }
    public void FindConnectedBlocks(Block currentBlock, List<Block> connectedBlocks)
    {
        Vector2Int[] directions = { new Vector2Int(currentBlock.Y + 1, currentBlock.X),
            new Vector2Int(currentBlock.Y - 1, currentBlock.X),
            new Vector2Int(currentBlock.Y, currentBlock.X - 1),
            new Vector2Int(currentBlock.Y, currentBlock.X + 1) };

        foreach (Vector2Int direction in directions)
        {

            if (direction.y == -1 || direction.x == -1 || direction.y == blocks.GetLength(0) || direction.x == blocks.GetLength(1))
                continue;

            if (!blocks[direction.x, direction.y])
                continue;

            if (blocks[direction.x, direction.y].type == currentBlock.type && !blocks[direction.x, direction.y].IsMerged)
            {
                blocks[direction.x, direction.y].IsMerged = true;
                connectedBlocks.Add(blocks[direction.x, direction.y]);
                FindConnectedBlocks(blocks[direction.x, direction.y], connectedBlocks);
            }
        }
    }
    public void ResetIsMerged()
    {
        foreach (Block block in blocks)
        {
            if (!block)
                continue;
            if (block.type == BlockState.None)
                continue;
            block.IsMerged = false;
        }
    }

    private Vector2Int IsUpBlockEmpty(int y, int x)
    {
        if (y + 1 == blockIndexs.GetLength(0))
        {
            return new Vector2Int(y, x);
        }

        if (blockIndexs[y + 1, x] == 0)
        {
            blockIndexs[y + 1, x] = blockIndexs[y, x];
            blockIndexs[y, x] = 0;
            return IsUpBlockEmpty(++y, x);
        }
        else
        {
            return new Vector2Int(y, x);
        }
    }
    private Vector2Int IsDownBlockEmpty(int y, int x)
    {
        if (y - 1 == -1)
        {
            return new Vector2Int(y, x);
        }

        if (blockIndexs[y - 1, x] == 0)
        {
            blockIndexs[y - 1, x] = blockIndexs[y, x];
            blockIndexs[y, x] = 0;
            return IsDownBlockEmpty(--y, x);
        }
        else
        {
            return new Vector2Int(y, x);
        }
    }
    private Vector2Int IsRightBlockEmpty(int y, int x)
    {
        if (x + 1 == blockIndexs.GetLength(1))
        {
            return new Vector2Int(y, x);
        }

        if (blockIndexs[y, x + 1] == 0)
        {
            blockIndexs[y, x + 1] = blockIndexs[y, x];
            blockIndexs[y, x] = 0;
            return IsRightBlockEmpty(y, ++x);
        }
        else
        {
            return new Vector2Int(y, x);
        }
    }
    private Vector2Int IsLeftBlockEmpty(int y, int x)
    {
        if (x - 1 == -1)
        {
            return new Vector2Int(y, x);
        }

        if (blockIndexs[y, x - 1] == 0)
        {
            blockIndexs[y, x - 1] = blockIndexs[y, x];
            blockIndexs[y, x] = 0;
            return IsLeftBlockEmpty(y, --x);
        }
        else
        {
            return new Vector2Int(y, x);
        }
    }


}

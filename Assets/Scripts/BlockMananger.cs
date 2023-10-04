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
    Count
}

public class BlockMananger : MonoBehaviour
{
    public static BlockMananger Instance { get; private set; }

    private int[,] blockIndexs = new int[5, 5];
    private Vector2[,] indexPos = new Vector2[5, 5];
    private GameObject[,] blocks = new GameObject[5, 5];

    [SerializeField]
    private int initCount = 5;
    [SerializeField]
    private float posOffset = 0.18f;
    [SerializeField]
    private GameObject spadePrefab;
    [SerializeField]
    private GameObject diamondPrefab;
    [SerializeField]
    private GameObject heartPrefab;
    [SerializeField]
    private GameObject cloverPrefab;
    [SerializeField]
    private GameObject gridPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    private void Start()
    {
        GameObject panel = GameObject.FindGameObjectWithTag("GridPanel");
        for (int y = 0; y < blockIndexs.GetLength(0); ++y)
        {
            for(int x = 0; x < blockIndexs.GetLength(1); ++x)
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


        int ranBlockState =  Random.Range((int)BlockState.None + 1, (int)BlockState.Count);
        blockIndexs[y, x] = ranBlockState;
        GameObject newBlock = null;
        switch (ranBlockState)
        {
            case 1:
                newBlock = Instantiate(spadePrefab, indexPos[y, x], Quaternion.identity);
                break;
            case 2:
                newBlock = Instantiate(diamondPrefab, indexPos[y, x], Quaternion.identity);
                break;
            case 3:
                newBlock = Instantiate(heartPrefab, indexPos[y, x], Quaternion.identity);
                break;
            case 4:
                newBlock = Instantiate(cloverPrefab, indexPos[y, x], Quaternion.identity);
                break;
            default:
                newBlock = Instantiate(cloverPrefab, indexPos[y, x], Quaternion.identity);
                break;
        }
        blocks[y, x] = newBlock;
    }

    public void MoveBlocks(float swipeAngle)
    {
        if (Mathf.Abs(swipeAngle) < 45)     // Right
        {
            Debug.Log("RightSwipe");
            for (int y = 0; y < blockIndexs.GetLength(0); ++y)
            {
                for (int x = blockIndexs.GetLength(1) - 1; x >= 0; --x)
                {
                    if (blockIndexs[y, x] == 0)
                        continue;

                    Vector2Int moveIndex = IsRightBlockEmpty(y, x);
                    if (moveIndex.x != y || moveIndex.y != x)
                    {
                        blocks[y, x].transform.position = indexPos[moveIndex.x, moveIndex.y];
                        blocks[moveIndex.x, moveIndex.y] = blocks[y, x];
                        blocks[y, x] = null;
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
                        blocks[y, x].transform.position = indexPos[moveIndex.x, moveIndex.y];
                        blocks[moveIndex.x, moveIndex.y] = blocks[y, x];
                        blocks[y, x] = null;
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
                        blocks[y, x].transform.position = indexPos[moveIndex.x, moveIndex.y];
                        blocks[moveIndex.x, moveIndex.y] = blocks[y, x];
                        blocks[y, x] = null;
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
                    if(moveIndex.x != y || moveIndex.y != x)
                    {
                        blocks[y, x].transform.position = indexPos[moveIndex.x, moveIndex.y];
                        blocks[moveIndex.x, moveIndex.y] = blocks[y, x];
                        blocks[y, x] = null;
                    }
                }
            }
        }
    }

    private Vector2Int IsUpBlockEmpty(int y,  int x)
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
    private Vector2Int IsDownBlockEmpty(int y,  int x)
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
    private Vector2Int IsRightBlockEmpty(int y,  int x)
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
    private Vector2Int IsLeftBlockEmpty(int y,  int x)
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

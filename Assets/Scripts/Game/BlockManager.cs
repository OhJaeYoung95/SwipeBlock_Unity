using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum BlockPattern
{
    None,
    Spade,
    Diamond,
    Heart,
    Clover,
    Obstcle,
    Joker,
    Count
}

public enum SwipeDir
{
    None = -1,
    Right,
    Left,
    Down,
    Up
}

public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance { get; private set; }

    public int boardSize = 5;

    private int[,] blockIndexs;
    private Vector2[,] indexPos = new Vector2[5, 5];
    private Block[,] blocks = new Block[5, 5];

    List<List<Block>> comparePatternBlocks = new List<List<Block>>();
    List<Block> jokerList = new List<Block>();
    public List<Block> obsList = new List<Block>();

    SwipeDir swipeDir = SwipeDir.None;
    BlockPattern standardBlock = BlockPattern.None;

    [SerializeField]
    private int initCount = 5;
    [SerializeField]
    private int spawnCount = 1;
    private int failedMergeSpawnCount;
    private int spadeAttributeCount = 0;
    public int effectIndex = 0;
    public int comboCount = 0;


    [SerializeField]
    private float increasedTimerRate = 1f;
    [SerializeField]
    private float posOffset = 0.18f;
    [SerializeField]
    private float moveDuration = 1f;
    [SerializeField]
    private float acceleratedSpeed = 0.2f;
    [SerializeField]
    private float accelerationValue = 2f;

    private float moveStartTime = 0f;

    public float obsSpawnTimeRate;

    private bool isChainMerge = false;
    private bool isOnSpadeAttribute = false;
    private bool isCompare = false;
    private bool isMergedDuringSwipe = false;
    public bool isSpawnObstacle = false;

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
    private Block jokerPrefab;
    [SerializeField]
    private GameObject gridPrefab;

    [SerializeField]
    private GameObject gridPanel;

    private List<string> poolKeys = new List<string>();

    private string spadeBlockPoolKey = "SpadeBlockPool";
    private string diamondBlockPoolKey = "DiamondBlockPool";
    private string heartBlockPoolKey = "HeartBlockPool";
    private string cloverBlockPoolKey = "CloverBlockPool";
    private string obstacleBlockPoolKey = "ObstacleBlockPool";
    private string jokerBlockPoolKey = "JokerBlockPool";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            ReloadBoard();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance.ReloadBoard();
            Destroy(gameObject);
            return;
        }
    }

    public void ReloadBoard()
    {
        isSpawnObstacle = false;
        obsList.Clear();
        switch (GameData.CurrentStage)
        {
            case 0:
                initCount = 6;
                boardSize = 4;
                spawnCount = 2;
                failedMergeSpawnCount = 1;
                obsSpawnTimeRate = 0;
                break;
            case 1:
                initCount = 10;
                boardSize = 5;
                spawnCount = 3;
                failedMergeSpawnCount = 1;
                obsSpawnTimeRate = 0.25f;
                break;
            case 2:
                initCount = 10;
                boardSize = 5;
                spawnCount = 3;
                failedMergeSpawnCount = 2;
                obsSpawnTimeRate = 0.5f;
                break;
            default:
                GameData.CurrentStage = 0;
                initCount = 6;
                boardSize = 4;
                spawnCount = 2;
                failedMergeSpawnCount = 1;
                break;
        }

        blockIndexs = new int[boardSize, boardSize];
        indexPos = new Vector2[boardSize, boardSize];
        blocks = new Block[boardSize, boardSize];
        for (int y = 0; y < blockIndexs.GetLength(0); y++)
        {
            for (int x = 0; x < blockIndexs.GetLength(1); x++)
            {
                blockIndexs[y, x] = 0;
                blocks[y, x] = null;
            }
        }
        poolKeys.Add(spadeBlockPoolKey);
        poolKeys.Add(diamondBlockPoolKey);
        poolKeys.Add(heartBlockPoolKey);
        poolKeys.Add(cloverBlockPoolKey);
        poolKeys.Add(obstacleBlockPoolKey);
        poolKeys.Add(jokerBlockPoolKey);

        gridPanel = GameObject.FindGameObjectWithTag("GridPanel");

        float panelSize = gridPanel.transform.localScale.x;
        Vector2 panelPos = gridPanel.transform.position;
        float gridSize = 0f;

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;


        switch (boardSize)
        {
            case 4:
                gridSize = 0.2f;
                posOffset = 0.23f;
                break;
            case 5:
                gridSize = 0.15f;
                posOffset = 0.18f;
                break;
        }


        for (int y = 0; y < blockIndexs.GetLength(0); ++y)
        {
            for (int x = 0; x < blockIndexs.GetLength(1); ++x)
            {
                blockIndexs[y, x] = 0;
                indexPos[y, x] = (new Vector2(-(posOffset * 2) + posOffset * x, -(posOffset * 2) + posOffset * y) * panelSize) + panelPos;
                if (boardSize % 2 == 0)
                    indexPos[y, x] += new Vector2(panelSize * 0.5f, panelSize * 0.5f) * Mathf.Pow(0.5f, 2f);
                GameObject grid = Instantiate(gridPrefab, gridPanel.transform);
                grid.transform.localScale = new Vector3(gridSize, gridSize);
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
            CreateRandomBlock();
        }
    }
    private void CreateRandomBlock()
    {
        int y = 0;
        int x = 0;
        do
        {
            y = Random.Range(0, blockIndexs.GetLength(0));
            x = Random.Range(0, blockIndexs.GetLength(1));
        } while (blockIndexs[y, x] != 0);

        int ranBlockState;

        if (isSpawnObstacle)
            ranBlockState = Random.Range((int)BlockPattern.None + 1, (int)BlockPattern.Joker);
        else
            ranBlockState = Random.Range((int)BlockPattern.None + 1, (int)BlockPattern.Obstcle);

        float blockSize = 0f;
        switch (boardSize)
        {
            case 4:
                blockSize = 0.9f;
                break;
            case 5:
                blockSize = 0.7f;
                break;
        }

        blockIndexs[y, x] = ranBlockState;
        Block newBlock = ObjectPoolManager.Instance.GetObjectPool<Block>(poolKeys[ranBlockState - 1]);

        if (ranBlockState == 5)
            obsList.Add(newBlock);

        newBlock.transform.position = indexPos[y, x];
        newBlock.transform.localScale = new Vector2(blockSize, blockSize);
        blocks[y, x] = newBlock;
        blocks[y, x].SetIndex(y, x);
    }
    private void CreateRandomBlock(Block newBlock)
    {
        int y = 0;
        int x = 0;
        do
        {
            y = Random.Range(0, blockIndexs.GetLength(0));
            x = Random.Range(0, blockIndexs.GetLength(1));
        } while (blockIndexs[y, x] != 0);

        float blockSize = 0f;
        switch (boardSize)
        {
            case 4:
                blockSize = 0.9f;
                break;
            case 5:
                blockSize = 0.7f;
                break;
        }

        blockIndexs[y, x] = (int)newBlock.type;
        newBlock.transform.position = indexPos[y, x];
        newBlock.transform.localScale = new Vector2(blockSize, blockSize);
        blocks[y, x] = newBlock;
        blocks[y, x].SetIndex(y, x);
    }
    private void CreateObsBlock()
    {
        Block newObs = ObjectPoolManager.Instance.GetObjectPool<Block>(poolKeys[(int)BlockPattern.Obstcle - 1]);
        obsList.Add(newObs);
        CreateRandomBlock(newObs);
    }
    public IEnumerator MoveBlocks(float swipeAngle)
    {
        do
        {
            isChainMerge = false;
            if (Mathf.Abs(swipeAngle) < 45)     // Right
            {
                swipeDir = SwipeDir.Right;
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
                            StartCoroutine(MoveBlockCoroutine(y, x, moveIndex));
                        }
                    }
                }
            }
            else if (Mathf.Abs(swipeAngle) > 135)       // Left
            {
                swipeDir = SwipeDir.Left;

                for (int y = 0; y < blockIndexs.GetLength(0); ++y)
                {
                    for (int x = 0; x < blockIndexs.GetLength(1); ++x)
                    {
                        if (blockIndexs[y, x] == 0)
                            continue;

                        Vector2Int moveIndex = IsLeftBlockEmpty(y, x);
                        if (moveIndex.x != y || moveIndex.y != x)
                        {
                            StartCoroutine(MoveBlockCoroutine(y, x, moveIndex));
                        }
                    }
                }
            }
            else if (swipeAngle < -45 && swipeAngle > -135)     // Down
            {
                swipeDir = SwipeDir.Down;

                for (int y = 0; y < blockIndexs.GetLength(0); ++y)
                {
                    for (int x = 0; x < blockIndexs.GetLength(1); ++x)
                    {
                        if (blockIndexs[y, x] == 0)
                            continue;

                        Vector2Int moveIndex = IsDownBlockEmpty(y, x);
                        if (moveIndex.x != y || moveIndex.y != x)
                        {
                            StartCoroutine(MoveBlockCoroutine(y, x, moveIndex));
                        }
                    }
                }
            }
            else        // Up
            {
                swipeDir = SwipeDir.Up;

                for (int y = blockIndexs.GetLength(0) - 1; y >= 0; --y)
                {
                    for (int x = 0; x < blockIndexs.GetLength(1); ++x)
                    {
                        if (blockIndexs[y, x] == 0)
                            continue;
                        Vector2Int moveIndex = IsUpBlockEmpty(y, x);
                        if (moveIndex.x != y || moveIndex.y != x)
                        {
                            StartCoroutine(MoveBlockCoroutine(y, x, moveIndex));
                        }
                    }
                }
            }
            yield return StartCoroutine(MergeBlocksCoroutine());
            ResetIsMerged();
        } while (isChainMerge);

        isCompare = false;
        ScoreManager.Instance.IsScoreIncreaseByPattern = false;
        comboCount = 0;

        if (isOnSpadeAttribute)
            OnSpadeAttribute();

        if(!isMergedDuringSwipe && GameData.CurrentStage != 0)
        {
            for(int i = 0; i < failedMergeSpawnCount; ++i)
            {
                if (CheckFullBoard())
                    break;
                CreateObsBlock();
            }
        }

        isMergedDuringSwipe = false;

        for (int x = 0; x < spawnCount; ++x)
        {
            if (CheckFullBoard())
                break;
            CreateRandomBlock();
        }

        GameManager.Instance.IsMove = false;

        if (CheckFullBoard())
            StartCoroutine(GameOver());
    }

    public void MoveBlock(int y, int x, Vector2Int moveIndex)
    {
        blocks[y, x].transform.position = indexPos[moveIndex.x, moveIndex.y];
        blocks[moveIndex.x, moveIndex.y] = blocks[y, x];
        blocks[moveIndex.x, moveIndex.y].SetIndex(moveIndex.x, moveIndex.y);
        blocks[y, x] = null;
    }
    private IEnumerator MoveBlockCoroutine(int y, int x, Vector2Int moveIndex)
    {
        float elapsedTime = 0f;
        float acceleratedTime = 0f;
        moveStartTime = Time.time;
        Vector2 startPos = blocks[y, x].transform.position;
        Vector2 destPos = indexPos[moveIndex.x, moveIndex.y];
        blocks[moveIndex.x, moveIndex.y] = blocks[y, x];
        blocks[moveIndex.x, moveIndex.y].SetIndex(moveIndex.x, moveIndex.y);
        blocks[y, x] = null;

        while (elapsedTime < moveDuration)
        {
            acceleratedTime += Time.deltaTime * acceleratedSpeed;

            float t = Mathf.Clamp01(elapsedTime + acceleratedTime / moveDuration);
            t = 1.0f - Mathf.Pow(1.0f - t, accelerationValue);

            blocks[moveIndex.x, moveIndex.y].transform.position = Vector2.Lerp(startPos, destPos, t);
            elapsedTime = Time.time - moveStartTime;
            yield return null;
        }

        if(blocks[moveIndex.x, moveIndex.y] != null)
            blocks[moveIndex.x, moveIndex.y].transform.position = destPos;
    }
    private IEnumerator MergeBlocks()
    {
        ConvertJokerIndex();
        CheckPattern();
        yield return null;
        yield return StartCoroutine(TryMerge());
    }
    public void ConvertJokerIndex()
    {
        List<Block> connectedBlocks = new List<Block>();

        for (int y = 0; y < blockIndexs.GetLength(0); y++)
        {
            for (int x = 0; x < blockIndexs.GetLength(1); x++)
            {
                if (!blocks[y, x])
                    continue;
                if (blocks[y, x].type == BlockPattern.None ||
                    blocks[y, x].type == BlockPattern.Obstcle ||
                    blocks[y, x].type == BlockPattern.Joker ||
                    blocks[y, x].IsChcekIndex)
                    continue;

                blocks[y, x].IsChcekIndex = true;
                connectedBlocks.Add(blocks[y, x]);
                FindConnectedIndexs(blocks[y, x], connectedBlocks);

                if (connectedBlocks.Count > 2)
                {
                    foreach (Block block in connectedBlocks)
                    {
                        if (block.type == BlockPattern.Joker)
                        {
                            block.IsChcekIndex = false;
                            if (block.CurrentPattern >= (int)connectedBlocks[0].type)
                            {
                                    block.IsContainList = true;
                                    blockIndexs[block.Y, block.X] = (int)connectedBlocks[0].type;
                                    block.CurrentPattern = (int)connectedBlocks[0].type;
                            }
                        }
                    }
                }
                if (connectedBlocks.Count <= 2)
                {
                    foreach (Block block in connectedBlocks)
                    {
                        if (block.type == BlockPattern.Joker)
                        {
                            block.IsChcekIndex = false;
                            blockIndexs[block.Y, block.X] = block.CurrentPattern;
                        }
                    }
                }
                connectedBlocks.Clear();
            }
        }

    }

    public void CheckPattern()
    {
        List<Block> connectedBlocks = new List<Block>();

        for (int y = 0; y < blockIndexs.GetLength(0); y++)
        {
            for (int x = 0; x < blockIndexs.GetLength(1); x++)
            {
                if (!blocks[y, x])
                    continue;
                if (blocks[y, x].type == BlockPattern.None ||
                    blocks[y, x].type == BlockPattern.Obstcle ||
                    blocks[y, x].type == BlockPattern.Joker ||
                    blocks[y, x].IsMerged)
                    continue;
                blocks[y, x].IsMerged = true;
                connectedBlocks.Add(blocks[y, x]);
                FindConnectedBlocks(blocks[y, x], connectedBlocks);

                if (connectedBlocks.Count >= 2)
                {
                    isMergedDuringSwipe = true;
                    comparePatternBlocks.Add(new List<Block>(connectedBlocks));
                }
                connectedBlocks.Clear();
            }
        }

        if (comparePatternBlocks.Count > 0 && !isCompare)
        {
            isCompare = true;
            Block compareBlock = comparePatternBlocks[0][0];
            switch (swipeDir)
            {
                case SwipeDir.Right:
                    foreach (List<Block> blocks in comparePatternBlocks)
                    {
                        foreach (Block block in blocks)
                        {
                            if (compareBlock.X < block.X)
                            {
                                compareBlock = block;
                            }
                            else if (compareBlock.X == block.X)
                            {
                                if (compareBlock.Y > block.Y)
                                    compareBlock = block;
                            }
                        }
                    }
                    standardBlock = compareBlock.type;
                    break;
                case SwipeDir.Left:
                    foreach (List<Block> blocks in comparePatternBlocks)
                    {
                        foreach (Block block in blocks)
                        {
                            if (compareBlock.X > block.X)
                            {
                                compareBlock = block;
                            }
                            else if (compareBlock.X == block.X)
                            {
                                if (compareBlock.Y < block.Y)
                                    compareBlock = block;
                            }
                        }
                    }
                    standardBlock = compareBlock.type;
                    break;
                case SwipeDir.Down:
                    foreach (List<Block> blocks in comparePatternBlocks)
                    {
                        foreach (Block block in blocks)
                        {
                            if (compareBlock.Y > block.Y)
                            {
                                compareBlock = block;
                            }
                            else if (compareBlock.Y == block.Y)
                            {
                                if (compareBlock.X > block.X)
                                    compareBlock = block;
                            }
                        }
                    }
                    standardBlock = compareBlock.type;
                    break;
                case SwipeDir.Up:
                    foreach (List<Block> blocks in comparePatternBlocks)
                    {
                        foreach (Block block in blocks)
                        {
                            if (compareBlock.Y < block.Y)
                            {
                                compareBlock = block;
                            }
                            else if (compareBlock.Y == block.Y)
                            {
                                if (compareBlock.X < block.X)
                                    compareBlock = block;
                            }
                        }
                    }
                    standardBlock = compareBlock.type;
                    break;
            }
        }
    }
    public IEnumerator TryMerge()
    {
        foreach (List<Block> blockList in comparePatternBlocks)
        {
            isChainMerge = true;
            comboCount++;
            int connectedCount = blockList.Count;

            Vector3 center = GetCenter(blockList);

            // 블록 특성
            switch (blockList[0].type)
            {
                case BlockPattern.Spade:
                    if (blockList.Count >= 3)
                    {
                        isOnSpadeAttribute = true;
                        spadeAttributeCount++;
                    }
                    break;
                case BlockPattern.Diamond:
                    ScoreManager.Instance.IsScoreIncreaseByPattern = true;
                    break;
                case BlockPattern.Heart:
                    UIManager.Instance.IncreaseTimer(increasedTimerRate * (connectedCount - 1));
                    break;
            }


            // 점수
            float scoreTextValue = 0f;
            int check = 0;

            scoreTextValue += ScoreManager.Instance.AddScoreBase();
            scoreTextValue += ScoreManager.Instance.AddScoreByConnected(connectedCount);
            check++;
            if (blockList[0].type != standardBlock)
            {
                scoreTextValue += ScoreManager.Instance.AddScoreByComparePattern();
                check++;
            }

            if (comboCount > 1)
            {
                scoreTextValue += ScoreManager.Instance.AddScoreByCombo();
                check++;
            }

            PlayTextEffect(center, scoreTextValue);

            foreach (Block block in blockList)
            {
                if (block.type == BlockPattern.Joker)
                    jokerList.Remove(block);

                SoundManager.Instance.PlayMergeSound();
                PlayMergeEffect(block, blockList);
                blockIndexs[block.Y, block.X] = 0;
                ObjectPoolManager.Instance.ReturnObjectPool<Block>(poolKeys[(int)blocks[block.Y, block.X].type - 1], block);
                blocks[block.Y, block.X] = null;
            }
            yield return new WaitForSeconds(0.1f);
        }
        comparePatternBlocks.Clear();
        foreach (Block joker in jokerList)
        {
            joker.IsChcekIndex = false;
            joker.IsContainList = false;
            joker.CurrentPattern = (int)BlockPattern.Joker;
            blockIndexs[joker.Y, joker.X] = (int)BlockPattern.Joker;
        }
    }
    public void ClearBoard()
    {
        for (int y = 0; y < blocks.GetLength(0); y++)
        {
            for (int x = 0; x < blocks.GetLength(1); x++)
            {
                if (blockIndexs[y, x] == 0)
                    continue;

                blockIndexs[y, x] = 0;
                ObjectPoolManager.Instance.ReturnObjectPool<Block>(poolKeys[(int)blocks[y, x].type - 1], blocks[y, x]);
                blocks[y, x] = null;
            }
        }
    }
    IEnumerator MergeBlocksCoroutine()
    {
        yield return new WaitForSeconds(0.1f + moveDuration);
        yield return StartCoroutine(MergeBlocks());
    }
    public void PlayMergeEffect(Block block, List<Block> blockList)
    {
        if (effectIndex == 0)
        {
            ParticleSystem effect = Instantiate(block.mergeEffect1, block.transform.position, Quaternion.identity);
            if (isOnSpadeAttribute && block.type == BlockPattern.Spade && blockList.Count >= 3)
            {
                ApplyMergeEffectColor(effect);
            }
            effect.gameObject.SetActive(true);
            effect.Play();
        }
        else
        {
            ParticleSystem effect = Instantiate(block.mergeEffect2, block.transform.position, Quaternion.identity);
            effect.gameObject.SetActive(true);
            effect.Play();
        }
    }
    public void FindConnectedIndexs(Block currentBlock, List<Block> connectedBlocks)
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

            if ((blockIndexs[direction.x, direction.y] > blockIndexs[currentBlock.Y, currentBlock.X] &&
                blocks[direction.x, direction.y].type == BlockPattern.Joker) && !blocks[direction.x, direction.y].IsChcekIndex)
            {
                blocks[direction.x, direction.y].IsChcekIndex = true;
                blockIndexs[direction.x, direction.y] = blockIndexs[currentBlock.Y, currentBlock.X];
                connectedBlocks.Add(blocks[direction.x, direction.y]);
                FindConnectedIndexs(blocks[direction.x, direction.y], connectedBlocks);
            }

            if ((blockIndexs[direction.x, direction.y] == blockIndexs[currentBlock.Y, currentBlock.X] 
                //|| blockIndexs[direction.x, direction.y] == (int)BlockPattern.Joker) 
                &&
                !blocks[direction.x, direction.y].IsChcekIndex /*|| blocks[direction.x, direction.y].IsContainList*/))
            {
                blocks[direction.x, direction.y].IsChcekIndex = true;
                blockIndexs[direction.x, direction.y] = blockIndexs[currentBlock.Y, currentBlock.X];
                connectedBlocks.Add(blocks[direction.x, direction.y]);
                FindConnectedIndexs(blocks[direction.x, direction.y], connectedBlocks);
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

            if (blockIndexs[direction.x, direction.y] == 0)
                continue;

            if (blockIndexs[direction.x, direction.y] == blockIndexs[currentBlock.Y, currentBlock.X] &&
                !blocks[direction.x, direction.y].IsMerged)
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
            if (block.type == BlockPattern.None)
                continue;
            block.IsMerged = false;
            block.IsChcekIndex = false;
            block.IsContainList = false;
            block.CurrentPattern = (int)block.type;
            blockIndexs[block.Y, block.X] = (int)block.type;
        }
    }
    public bool CheckFullBoard()
    {
        for (int y = 0; y < blockIndexs.GetLength(0); ++y)
        {
            for (int x = 0; x < blockIndexs.GetLength(1); ++x)
            {
                if (blockIndexs[y, x] == 0)
                    return false;
            }
        }
        return true;
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.GameOver();
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

    private void OnSpadeAttribute()
    {
        isOnSpadeAttribute = false;
        for (int i = 0; i < spadeAttributeCount; ++i)
        {
            if (CheckFullBoard())
            {
                GameManager.Instance.IsMove = false;
                StartCoroutine(GameOver());
                return;
            }
            Block newJoker = ObjectPoolManager.Instance.GetObjectPool<Block>(poolKeys[(int)BlockPattern.Joker - 1]);
            jokerList.Add(newJoker);
            CreateRandomBlock(newJoker);
        }
        spadeAttributeCount = 0;
    }

    public void RemoveBlock(Block block)
    {
        if (blockIndexs[block.Y, block.X] == 0)
            return;

        blockIndexs[block.Y, block.X] = 0;
        ObjectPoolManager.Instance.ReturnObjectPool<Block>(poolKeys[(int)blocks[block.Y, block.X].type - 1], blocks[block.Y, block.X]);
        blocks[block.Y, block.X] = null;
    }

    public void ApplyMergeEffectColor(ParticleSystem effect)
    {
        Color newColor = new Color(60 / 255f, 60 / 255f, 60 / 255f);
        var mainModule = effect.main;
        mainModule.startColor = newColor;

        ParticleSystem effect1 = effect.transform.GetChild(0).GetComponent<ParticleSystem>();
        ApplytMergeColor(effect1, newColor);

        ParticleSystem effect2 = effect.transform.GetChild(1).GetComponent<ParticleSystem>();
        ApplytMergeColor(effect2, newColor);

        ParticleSystem effect3 = effect.transform.GetChild(2).GetComponent<ParticleSystem>();
        ApplytMergeColor(effect3, newColor);

        ParticleSystem effect4 = effect.transform.GetChild(3).GetComponent<ParticleSystem>();
        ApplytMergeColor(effect4, newColor);

        //Light light = effect.transform.GetChild(4).GetComponent<Light>();
        //light.color = newColor;
    }

    public void ApplytMergeColor(ParticleSystem effect, Color value)
    {
        var mainModule = effect.main;
        mainModule.startColor = value;
    }

    public Vector3 GetCenter(List<Block> blockList)
    {
        Vector3 totalPosition = Vector3.zero;

        foreach (Block block in blockList)
        {
            totalPosition += block.transform.position;
        }

        Vector3 center = totalPosition / blockList.Count;
        return center;
    }

    public void PlayTextEffect(Vector3 center, float value)
    {
        SoundManager.Instance.PlayScoreUpSound();
        TextMeshProUGUI scoreText = ObjectPoolManager.Instance.GetObjectPool<TextEffect>("ScoreTextPool").GetComponent<TextMeshProUGUI>();
        if(ScoreManager.Instance.IsScoreIncreaseByItem)
        {
            scoreText.color = Color.green;
        }

        if(ScoreManager.Instance.IsScoreIncreaseByPattern)
        {
            scoreText.color = Color.blue;
        }

        if(ScoreManager.Instance.IsScoreIncreaseByItem && ScoreManager.Instance.IsScoreIncreaseByPattern)
        {
            scoreText.color = Color.yellow;
        }

        scoreText.text = $"{value}";
        scoreText.transform.SetParent(ObjectPoolManager.Instance.canvas.transform.GetChild(0));

        RectTransform canvasRect = UIManager.Instance.foreCanvas.GetComponent<RectTransform>();
        scoreText.rectTransform.localScale = Vector3.one;

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(center)/* - new Vector3(offsetX, offsetY)*/;

        scoreText.rectTransform.anchoredPosition = new Vector3(((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
    }
}
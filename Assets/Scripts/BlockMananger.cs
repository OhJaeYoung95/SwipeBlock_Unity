using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMananger : MonoBehaviour
{
    public BlockMananger Instance { get; private set; }

    public int[,] blockIndexs = new int[5, 5];
    public Vector2[,] indexPos = new Vector2[5, 5];
    public GameObject[,] blocks = new GameObject[5, 5];

    public GameObject blockPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int y = 0; y < blockIndexs.GetLength(0); ++y)
        {
            for(int x = 0; x < blockIndexs.GetLength(1); ++x)
            {
                blockIndexs[y, x] = 0;
                indexPos[y, x] = new Vector2(x - 2, y - 2);
            }
        }
        RandomCreate();
    }

    void RandomCreate()
    {
        int y = Random.Range(0, blockIndexs.GetLength(0));
        int x = Random.Range(0, blockIndexs.GetLength(1));

        blockIndexs[y, x] = 1;
        GameObject newBlock = Instantiate(blockPrefab, indexPos[y, x], Quaternion.identity);
    }
}

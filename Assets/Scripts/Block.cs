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
}
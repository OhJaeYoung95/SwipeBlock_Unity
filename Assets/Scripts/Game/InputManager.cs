using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private Vector2 mouseDownPos;
    private Vector2 mouseUpPos;
    private Vector2 mouseDragPos;
    private float maxSwipeDistance = 50f;
    private float minSwipeDistance = 0.1f;
    [SerializeField]
    private float sensitivityDistance = 2f;
    private bool isSwiping = false;
    public bool isHover = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TrySwipeInPC();



        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //GameData.Gold = 10000;
        //    //GameManager.Instance.GameOver();
        //    UIManager.Instance.gameTimer -= 10f;
        //}
    }

    private void TrySwipeInPC()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.IsGameOver && !GameManager.Instance.IsPause && !isHover)
        {
            mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && !isSwiping && !GameManager.Instance.IsGameOver && !GameManager.Instance.IsPause && !isHover)
        {
            mouseDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DetectSwipeLatestVersion();
        }

        if (Input.GetMouseButtonUp(0) && !GameManager.Instance.IsGameOver && !GameManager.Instance.IsPause && !isHover)
        {
            isSwiping = false;
        }
    }

    private void DetectSwipeLatestVersion()
    {
        Vector2 swipeDir = mouseDragPos - mouseDownPos;
        float swipeDistance = swipeDir.magnitude;
        if (swipeDistance < sensitivityDistance)
        {
            return;
        }

        if(swipeDistance > sensitivityDistance && !GameManager.Instance.IsMove)
        {
            isSwiping = true;
            GameManager.Instance.IsMove = true;
            float swipeAngle = Mathf.Atan2(swipeDir.y, swipeDir.x) * Mathf.Rad2Deg;
            StartCoroutine(BlockManager.Instance.MoveBlocks(swipeAngle));
        }
    }
    private void DetectSwipePreviousVersion()
    {
        Vector2 swipeDir = mouseUpPos - mouseDownPos;
        float swipeDistance = swipeDir.magnitude;

        if (swipeDistance < minSwipeDistance)
        {
            GameManager.Instance.IsMove = false;
            return;
        }

        if (swipeDistance < maxSwipeDistance)
        {
            float swipeAngle = Mathf.Atan2(swipeDir.y, swipeDir.x) * Mathf.Rad2Deg;
            StartCoroutine(BlockManager.Instance.MoveBlocks(swipeAngle));
        }
    }

    public void OnButtonHoverEnter()
    {
        isHover = true;
    }

    public void OnButtonHoverExit()
    {
        isHover = false;
    }
}
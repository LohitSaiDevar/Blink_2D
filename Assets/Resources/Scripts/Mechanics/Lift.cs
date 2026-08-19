using UnityEngine;
enum LiftType
{
    Vertical,
    Horizontal
}

enum LiftDirection
{
    Up,
    Down,
    Left,
    Right
}

public class Lift : MonoBehaviour
{
    [SerializeField] LiftType currentType;
    [SerializeField] LiftDirection currentDirection;
    [SerializeField] Transform posA, posB;
    [SerializeField] Vector3 targetPos;
    Rigidbody2D rb;
    [SerializeField] float speed;
    bool isActivated;
    GameObject player;
    [SerializeField] Transform initialPos;

    public bool IsPlayerOnThisLift { get; private set; }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPos = Vector3.zero;
        transform.position = initialPos.position;
    }

    private void Update()
    {
        HandleLiftMovement(currentType);
    }

    private void OnEnable()
    {
        GameManager.OnPlayerRespawn += ResetLift;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerRespawn -= ResetLift;
    }

    void HandleLiftMovement(LiftType type)
    {
        if (isActivated)
        {
            switch (type)
            {
                // Move lift vertically
                case LiftType.Vertical:

                    if (currentDirection == LiftDirection.Up)
                    {
                        MoveLift(posA.position);

                    }
                    else if (currentDirection == LiftDirection.Down)
                    {
                        MoveLift(posB.position);
                    }
                    break;

                // Move lift horizontally
                case LiftType.Horizontal:

                    if (currentDirection == LiftDirection.Left)
                    {
                        MoveLift(posA.position);
                    }
                    else if (currentDirection == LiftDirection.Right)
                    {
                        MoveLift(posB.position);
                    }
                    break;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActivated = true;
            player = collision.gameObject;
            player.transform.SetParent(transform);
            //Debug.Log("Is player on lift: " + IsPlayerOnThisLift);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.transform.SetParent(null);
            player = null;
        }
    }

    void MoveLift(Vector3 targetPos)
    {
        speed = 2;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    void ResetLift()
    {
        isActivated = false;
        speed = 0;
        transform.position = initialPos.position;
    }
}

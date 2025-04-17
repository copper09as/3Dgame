using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform[] groundCheckPoints;
    [SerializeField] private GameObject flyBird;

    [SerializeField] private DialogueBox _dialogueBox; // 引用对话框
    private bool _inputEnabled = true;

    private PlayerStateMachine stateMachine;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;

    private int groundedCount;
    private int waterCount;
    public bool inWater = false;
    private bool isGrounded = true;
    private bool isWater = false;

    void Start()
    {
        Init();

        if (_dialogueBox != null)
        {
            _dialogueBox.OnClosed += OnDialogueCancelled;
        }

    }

    private void Init()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        stateMachine = new PlayerStateMachine(this, animator, rb);
        stateMachine.Init(playerState.Idle);
    }

    public void Move(Vector3 direction)
    {
        if (!_inputEnabled) return;

        rb.velocity = direction;
        transform.forward = Vector3.Slerp(transform.forward, direction, GameApp.Instance.playerData.turnSpeed);
    }

    void Update()
    {
        if (!_inputEnabled) return;

        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        UpdateGroundedStatus();
    }

    public void SetInputEnabled(bool enabled)
    {
        _inputEnabled = enabled;

        if (!enabled)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnDialogueCancelled()
    {
        SetInputEnabled(true);
    }

    // 其他方法保持不变 ↓↓↓

    public bool IsGrounded() => isGrounded;
    public Rigidbody GetRb() => rb;
    public bool IsFish() => isWater;
    public GameObject GetBird() => flyBird;
    public bool IsInWater() => inWater;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fish")
        {
            isWater = true;
        }
        if (other.tag == "Water")
        {
            inWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Fish")
        {
            isWater = false;
        }
        if (other.tag == "Water")
        {
            inWater = false;
        }
    }

    private void UpdateGroundedStatus()
    {
        groundedCount = 0;
        foreach (var point in groundCheckPoints)
        {
            Collider[] colliders = Physics.OverlapSphere(
                point.position,
                groundCheckRadius,
                groundLayer
            );

            if (colliders.Length > 0)
            {
                groundedCount++;
            }
        }

        isGrounded = groundedCount > 0;
    }
}

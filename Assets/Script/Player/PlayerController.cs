using UnityEngine;
public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform[] groundCheckPoints;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private float turnSpeed = 0.3f;
    [SerializeField] private GameObject flyBird;
    [SerializeField] private float moveSpeed = 4f;
    private PlayerStateMachine stateMachine;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;
    int groundedCount;
    void Start()
    {
        Init();
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
        rb.velocity = direction * moveSpeed;
        transform.forward = Vector3.Slerp(transform.forward, direction, turnSpeed);
    }
    void Update()
    {
        stateMachine.Update();
    }
    public bool IsGrounded() => isGrounded;
    public Rigidbody GetRb() => rb;

    public GameObject GetBird() => flyBird;
    private void FixedUpdate()
    {
        UpdateGroundedStatus();
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

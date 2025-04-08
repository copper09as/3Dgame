using System;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  // 普通行走速度
    public float runSpeed = 10f;  // 奔跑时的速度
    public float turnSpeed = 0.3f;
    public Transform cameraTransform;
    [SerializeField] private CameraFollow cameraFollow;
    public Animator animator;
    public Rigidbody rb;
    [SerializeField]private bool isGrounded = true;
    private PlayerStateMachine stateMachine;
    private Vector3 moveDirection;
    [SerializeField] private LayerMask groundLayer; // 在Inspector中设置地面层
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform[] groundCheckPoints; // 多个检测点
    int groundedCount;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        stateMachine = new PlayerStateMachine(this);
        stateMachine.Init(new PlayerIdle(stateMachine,animator));

    }
public void Move(Vector3 direction)
    {
        rb.velocity = direction; 
        transform.forward = Vector3.Slerp(transform.forward, direction, turnSpeed);
    }
    void Update()
    {
        stateMachine.Update();
    }
    public bool IsGrounded() => isGrounded;
    private void FixedUpdate()
    {
        UpdateGroundedStatus();
        OnDrawGizmosSelected();
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

    // 可选：可视化检测点
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var point in groundCheckPoints)
        {
            Gizmos.DrawWireSphere(point.position, groundCheckRadius);
        }
    }
}

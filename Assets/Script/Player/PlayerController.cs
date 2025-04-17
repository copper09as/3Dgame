using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform[] groundCheckPoints;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private GameObject flyBird;
    [SerializeField] private bool isWater = false;
    private PlayerStateMachine stateMachine;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;
    int groundedCount;
    int waterCount;
    float vertical;
    float horizontal;
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
        rb.velocity = direction;
        if(vertical >= 0 && horizontal != 0)
            transform.forward = Vector3.Slerp(transform.forward, direction,GameApp.Instance.playerData.turnSpeed);
    }
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        stateMachine.Update();
    }
    public bool inWater = false;
    public bool IsGrounded() => isGrounded;
    public Rigidbody GetRb() => rb;
    public bool IsFish() => isWater;
    public GameObject GetBird() => flyBird;
    private void FixedUpdate()
    {
        UpdateGroundedStatus();
    }
    public bool IsInWater() => inWater;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Fish")
        {
            isWater = true;
        }
        if(other.tag == "Water")
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
        waterCount = 0;
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

using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("游泳参数")]
    public float swimGravity = -1f;
    public float swimSpeed = 3f;
    public float waterSurfaceY = 1.5f;

    [Header("其他")]
    public float turnSpeed = 120f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isSwimming = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (isSwimming)
        {
            SwimUpdate(horizontal, vertical);
            return;
        }

        GroundedMovement(horizontal, vertical);
    }

    void SwimUpdate(float horizontal, float vertical)
    {
        Vector3 moveDir = transform.forward * vertical;

        if (Input.GetKey(KeyCode.Space))
            moveDir += Vector3.up;
        if (Input.GetKey(KeyCode.LeftControl))
            moveDir += Vector3.down;

        // 左右旋转角色
        if (horizontal != 0f)
        {
            float turnAmount = horizontal * turnSpeed * Time.deltaTime;
            transform.Rotate(0f, turnAmount, 0f);
        }

        controller.Move(moveDir.normalized * swimSpeed * Time.deltaTime);

        // 自动浮到水面
        if (transform.position.y < waterSurfaceY - 0.3f)
        {
            controller.Move(Vector3.up * swimSpeed * 0.5f * Time.deltaTime);
        }

        // 动画设置
        animator.SetBool("IsSwimming", true);
        animator.SetFloat("Speed", moveDir.magnitude);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
    }

    void GroundedMovement(float horizontal, float vertical)
    {
        // 地面检测与重力
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? runSpeed : moveSpeed;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

            animator.SetFloat("Speed", currentSpeed);
            animator.SetFloat("IsRunning", currentSpeed == runSpeed ? 1f : 0f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            animator.SetFloat("IsRunning", 0f);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("IsJumping", true);
        }

        velocity.y += gravity * Time.deltaTime;

        if (!isGrounded && velocity.y < -1f)
        {
            animator.SetBool("IsFalling", true);
        }

        controller.Move(velocity * Time.deltaTime);

        animator.SetBool("IsSwimming", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isSwimming = true;
            velocity = Vector3.zero;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isSwimming = false;
            animator.SetBool("IsSwimming", false);
        }
    }
}

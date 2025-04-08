using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  // 普通行走速度
    public float runSpeed = 10f;  // 奔跑时的速度
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float turnSpeed = 10f;  // 旋转速度
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Ground Check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("IsJumping", false);
        }

        // Get Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // 动画和角色控制
        if (direction.magnitude >= 0.1f)
        {
            // 计算目标朝向角度
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // 使角色平滑转动到目标角度
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);  // 使用Slerp使旋转更加平滑

            // Move direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // 判断是否按下 Shift 键来决定是否奔跑
            float currentMoveSpeed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? runSpeed : moveSpeed;

            // 使用当前速度进行移动
            controller.Move(moveDir.normalized * currentMoveSpeed * Time.deltaTime);

            // Animation - Speed (用于行走/跑步)
            float currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
            animator.SetFloat("Speed", currentSpeed);

            // 动画控制 - 使用浮动值来控制奔跑
            float runningValue = (currentMoveSpeed == runSpeed) ? 1f : 0f;  // 1表示奔跑，0表示不奔跑
            animator.SetFloat("IsRunning", currentSpeed);  // 控制奔跑动画
        }
        else
        {
            // 如果没有输入，确保 Speed 为 0
            animator.SetFloat("Speed", 0f);
            animator.SetFloat("IsRunning", 0f);  // 角色停止时，停止奔跑动画
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("IsJumping", true);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

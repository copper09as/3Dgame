using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  // ��ͨ�����ٶ�
    public float runSpeed = 10f;  // ����ʱ���ٶ�
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float turnSpeed = 10f;  // ��ת�ٶ�
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

        // �����ͽ�ɫ����
        if (direction.magnitude >= 0.1f)
        {
            // ����Ŀ�곯��Ƕ�
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // ʹ��ɫƽ��ת����Ŀ��Ƕ�
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);  // ʹ��Slerpʹ��ת����ƽ��

            // Move direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // �ж��Ƿ��� Shift ���������Ƿ���
            float currentMoveSpeed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? runSpeed : moveSpeed;

            // ʹ�õ�ǰ�ٶȽ����ƶ�
            controller.Move(moveDir.normalized * currentMoveSpeed * Time.deltaTime);

            // Animation - Speed (��������/�ܲ�)
            float currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
            animator.SetFloat("Speed", currentSpeed);

            // �������� - ʹ�ø���ֵ�����Ʊ���
            float runningValue = (currentMoveSpeed == runSpeed) ? 1f : 0f;  // 1��ʾ���ܣ�0��ʾ������
            animator.SetFloat("IsRunning", currentSpeed);  // ���Ʊ��ܶ���
        }
        else
        {
            // ���û�����룬ȷ�� Speed Ϊ 0
            animator.SetFloat("Speed", 0f);
            animator.SetFloat("IsRunning", 0f);  // ��ɫֹͣʱ��ֹͣ���ܶ���
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

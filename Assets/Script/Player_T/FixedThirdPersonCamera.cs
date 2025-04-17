using UnityEngine;

public class FixedThirdPersonCamera : MonoBehaviour
{
    [Header("����Ŀ��")]
    public Transform target;

    [Header("��������")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5); // Ĭ�ϵ����ӽ�
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float lookAtHeight = 1.5f; // ���߽���߶�

    [Header("��ײ���")]
    [SerializeField] private float collisionRadius = 0.2f;
    [SerializeField] private LayerMask obstacleLayers;

    private Vector3 velocity = Vector3.zero;
    private float originalDistance;

    void Start()
    {
        originalDistance = offset.magnitude;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // �������λ��
        Vector3 desiredPosition = CalculateDesiredPosition();

        // ��ײ������
        desiredPosition = AdjustForCollision(desiredPosition);

        // ƽ���ƶ�
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );

        // ƽ��ע��Ŀ�꣨������ֱƫ�ƣ�
        Quaternion targetRotation = Quaternion.LookRotation(
            (target.position + Vector3.up * lookAtHeight) - transform.position
        );
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 10f
        );
    }

    Vector3 CalculateDesiredPosition()
    {
        return target.TransformPoint(offset);
    }

    Vector3 AdjustForCollision(Vector3 desiredPos)
    {
        Vector3 direction = desiredPos - target.position;
        float targetDistance = direction.magnitude;

        if (Physics.SphereCast(
            target.position,
            collisionRadius,
            direction.normalized,
            out RaycastHit hit,
            targetDistance,
            obstacleLayers))
        {
            return hit.point - direction.normalized * collisionRadius;
        }
        return desiredPos;
    }

    // ��̬��������������ʾ����
    public void SetCameraOffset(Vector3 newOffset)
    {
        offset = newOffset;
        originalDistance = offset.magnitude;
    }
}

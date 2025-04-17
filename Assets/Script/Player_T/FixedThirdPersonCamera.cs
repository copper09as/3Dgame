using UnityEngine;

public class FixedThirdPersonCamera : MonoBehaviour
{
    [Header("跟随目标")]
    public Transform target;

    [Header("基础参数")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5); // 默认第三视角
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float lookAtHeight = 1.5f; // 视线焦点高度

    [Header("碰撞检测")]
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

        // 计算基础位置
        Vector3 desiredPosition = CalculateDesiredPosition();

        // 碰撞检测调整
        desiredPosition = AdjustForCollision(desiredPosition);

        // 平滑移动
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );

        // 平滑注视目标（包含垂直偏移）
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

    // 动态调整参数方法（示例）
    public void SetCameraOffset(Vector3 newOffset)
    {
        offset = newOffset;
        originalDistance = offset.magnitude;
    }
}

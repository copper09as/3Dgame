using UnityEngine;
using System.Collections;

public class AdvancedFishMovement : MonoBehaviour
{
    [Header("水域设置")]
    public Collider waterCollider; // 支持BoxCollider和MeshCollider
    public float swimHeight = 2f;

    [Header("移动参数")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 8f;
    public float turnSmoothTime = 0.5f;

    [Header("行为参数")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;
    public float wallCheckDistance = 0.5f;
    public LayerMask obstacleLayers;

    // 运行状态
    private Vector3 currentTarget;
    private bool isTurning;
    private Rigidbody rb;
    private Transform fishTransform;
    private float turnProgress;

    // 卡住检测参数
    private Vector3 lastPosition;
    private float stuckTime = 2f; // 卡住时间阈值
    private float timeStuck = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fishTransform = transform;
        rb.useGravity = false;
        GenerateNewTarget();
        lastPosition = rb.position;
    }

    void Update()
    {
        if (!isTurning)
        {
            if (CheckObstacleAhead())
            {
                StartCoroutine(PerformAvoidanceTurn());
            }
            else
            {
                MoveTowardsTarget();
            }

            // 检查是否卡住
            CheckIfStuck();
        }
    }

    void MoveTowardsTarget()
    {
        // 保持高度
        Vector3 currentPos = rb.position;
        currentPos.y = swimHeight;
        rb.position = currentPos;

        // 计算移动方向
        Vector3 direction = (currentTarget - currentPos).normalized;

        // 仅当有有效方向时移动
        if (direction != Vector3.zero)
        {
            // 平滑转向
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            fishTransform.rotation = Quaternion.Slerp(
                fishTransform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // 移动
            float moveStep = Mathf.Min(moveSpeed * Time.deltaTime, Vector3.Distance(currentPos, currentTarget));
            rb.MovePosition(currentPos + fishTransform.forward * moveStep);

            // 到达目标点
            if (Vector3.Distance(currentPos, currentTarget) < 0.1f)
            {
                StartCoroutine(WaitAndFindNewTarget());
            }
        }
    }

    IEnumerator PerformAvoidanceTurn()
    {
        isTurning = true;

        // 计算新的安全方向
        Vector3 safeDirection = CalculateSafeDirection();
        Quaternion startRotation = fishTransform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(safeDirection);

        float turnDuration = Vector3.Angle(fishTransform.forward, safeDirection) / rotationSpeed;
        float elapsed = 0f;

        while (elapsed < turnDuration)
        {
            fishTransform.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                elapsed / turnDuration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 转向完成后生成新目标
        GenerateNewTarget();
        isTurning = false;
    }

    Vector3 CalculateSafeDirection()
    {
        // 8方向射线检测寻找安全路径
        float[] angles = { 0f, 45f, -45f, 90f, -90f, 135f, -135f, 180f };
        foreach (float angle in angles)
        {
            Vector3 testDir = Quaternion.Euler(0, angle, 0) * fishTransform.forward;
            if (!Physics.Raycast(rb.position, testDir, wallCheckDistance, obstacleLayers))
            {
                return testDir;
            }
        }
        return -fishTransform.forward; // 后退作为最后手段
    }

    void GenerateNewTarget()
    {
        Vector3 newTarget;
        int attempts = 0;
        do
        {
            newTarget = GetRandomPointInCollider();
            attempts++;
        } while (attempts < 100 && Vector3.Distance(rb.position, newTarget) < 1f);

        currentTarget = newTarget;
    }

    Vector3 GetRandomPointInCollider()
    {
        if (waterCollider is BoxCollider box)
        {
            // 考虑旋转的BoxCollider处理
            Vector3 localPoint = new Vector3(
                Random.Range(-box.size.x / 2, box.size.x / 2),
                swimHeight - box.transform.position.y,
                Random.Range(-box.size.z / 2, box.size.z / 2)
            );

            return box.transform.TransformPoint(localPoint);
        }
        else if (waterCollider is MeshCollider mesh)
        {
            // 适用于复杂形状的采样算法
            return GetRandomPointInMesh(mesh);
        }

        return rb.position + Random.insideUnitSphere * 5f;
    }

    Vector3 GetRandomPointInMesh(MeshCollider meshCollider)
    {
        Mesh mesh = meshCollider.sharedMesh;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        // 随机选择一个三角形
        int index = Random.Range(0, triangles.Length / 3) * 3;
        Vector3 p1 = vertices[triangles[index]];
        Vector3 p2 = vertices[triangles[index + 1]];
        Vector3 p3 = vertices[triangles[index + 2]];

        // 生成随机点
        float u = Random.value;
        float v = Random.value * (1 - u);
        Vector3 point = p1 + u * (p2 - p1) + v * (p3 - p1);

        // 转换到世界坐标
        return meshCollider.transform.TransformPoint(point);
    }

    bool CheckObstacleAhead()
    {
        return Physics.SphereCast(
            rb.position,
            0.3f,
            fishTransform.forward,
            out _,
            wallCheckDistance,
            obstacleLayers
        );
    }

    IEnumerator WaitAndFindNewTarget()
    {
        isTurning = true;
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);
        GenerateNewTarget();
        isTurning = false;
    }

    void CheckIfStuck()
    {
        // 检测鱼是否卡住
        if (Vector3.Distance(lastPosition, rb.position) < 0.1f)
        {
            timeStuck += Time.deltaTime;
        }
        else
        {
            timeStuck = 0f;
        }

        // 如果卡住超过2秒，重新选择目标
        if (timeStuck >= stuckTime)
        {
            GenerateNewTarget();
            timeStuck = 0f; // 重置卡住时间
        }

        lastPosition = rb.position; // 更新位置
    }

    void OnDrawGizmosSelected()
    {
        // 绘制水域边界
        if (waterCollider != null)
        {
            Gizmos.color = new Color(0, 1, 1, 0.3f);
            if (waterCollider is BoxCollider box)
            {
                Matrix4x4 originalMatrix = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS(
                    box.transform.position,
                    box.transform.rotation,
                    box.transform.lossyScale
                );
                Gizmos.DrawWireCube(box.center, box.size);
                Gizmos.matrix = originalMatrix;
            }
            else if (waterCollider is MeshCollider mesh)
            {
                Gizmos.DrawWireMesh(mesh.sharedMesh, mesh.transform.position, mesh.transform.rotation, mesh.transform.lossyScale);
            }
        }

        // 绘制当前目标
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(currentTarget, 0.3f);

        // 绘制前进方向
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
}

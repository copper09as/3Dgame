using UnityEngine;
using System.Collections;

public class AdvancedFishMovement : MonoBehaviour
{
    [Header("ˮ������")]
    public Collider waterCollider; // ֧��BoxCollider��MeshCollider
    public float swimHeight = 2f;

    [Header("�ƶ�����")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 8f;
    public float turnSmoothTime = 0.5f;

    [Header("��Ϊ����")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;
    public float wallCheckDistance = 0.5f;
    public LayerMask obstacleLayers;

    // ����״̬
    private Vector3 currentTarget;
    private bool isTurning;
    private Rigidbody rb;
    private Transform fishTransform;
    private float turnProgress;

    // ��ס������
    private Vector3 lastPosition;
    private float stuckTime = 2f; // ��סʱ����ֵ
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

            // ����Ƿ�ס
            CheckIfStuck();
        }
    }

    void MoveTowardsTarget()
    {
        // ���ָ߶�
        Vector3 currentPos = rb.position;
        currentPos.y = swimHeight;
        rb.position = currentPos;

        // �����ƶ�����
        Vector3 direction = (currentTarget - currentPos).normalized;

        // ��������Ч����ʱ�ƶ�
        if (direction != Vector3.zero)
        {
            // ƽ��ת��
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            fishTransform.rotation = Quaternion.Slerp(
                fishTransform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // �ƶ�
            float moveStep = Mathf.Min(moveSpeed * Time.deltaTime, Vector3.Distance(currentPos, currentTarget));
            rb.MovePosition(currentPos + fishTransform.forward * moveStep);

            // ����Ŀ���
            if (Vector3.Distance(currentPos, currentTarget) < 0.1f)
            {
                StartCoroutine(WaitAndFindNewTarget());
            }
        }
    }

    IEnumerator PerformAvoidanceTurn()
    {
        isTurning = true;

        // �����µİ�ȫ����
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

        // ת����ɺ�������Ŀ��
        GenerateNewTarget();
        isTurning = false;
    }

    Vector3 CalculateSafeDirection()
    {
        // 8�������߼��Ѱ�Ұ�ȫ·��
        float[] angles = { 0f, 45f, -45f, 90f, -90f, 135f, -135f, 180f };
        foreach (float angle in angles)
        {
            Vector3 testDir = Quaternion.Euler(0, angle, 0) * fishTransform.forward;
            if (!Physics.Raycast(rb.position, testDir, wallCheckDistance, obstacleLayers))
            {
                return testDir;
            }
        }
        return -fishTransform.forward; // ������Ϊ����ֶ�
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
            // ������ת��BoxCollider����
            Vector3 localPoint = new Vector3(
                Random.Range(-box.size.x / 2, box.size.x / 2),
                swimHeight - box.transform.position.y,
                Random.Range(-box.size.z / 2, box.size.z / 2)
            );

            return box.transform.TransformPoint(localPoint);
        }
        else if (waterCollider is MeshCollider mesh)
        {
            // �����ڸ�����״�Ĳ����㷨
            return GetRandomPointInMesh(mesh);
        }

        return rb.position + Random.insideUnitSphere * 5f;
    }

    Vector3 GetRandomPointInMesh(MeshCollider meshCollider)
    {
        Mesh mesh = meshCollider.sharedMesh;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        // ���ѡ��һ��������
        int index = Random.Range(0, triangles.Length / 3) * 3;
        Vector3 p1 = vertices[triangles[index]];
        Vector3 p2 = vertices[triangles[index + 1]];
        Vector3 p3 = vertices[triangles[index + 2]];

        // ���������
        float u = Random.value;
        float v = Random.value * (1 - u);
        Vector3 point = p1 + u * (p2 - p1) + v * (p3 - p1);

        // ת������������
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
        // ������Ƿ�ס
        if (Vector3.Distance(lastPosition, rb.position) < 0.1f)
        {
            timeStuck += Time.deltaTime;
        }
        else
        {
            timeStuck = 0f;
        }

        // �����ס����2�룬����ѡ��Ŀ��
        if (timeStuck >= stuckTime)
        {
            GenerateNewTarget();
            timeStuck = 0f; // ���ÿ�סʱ��
        }

        lastPosition = rb.position; // ����λ��
    }

    void OnDrawGizmosSelected()
    {
        // ����ˮ��߽�
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

        // ���Ƶ�ǰĿ��
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(currentTarget, 0.3f);

        // ����ǰ������
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
}

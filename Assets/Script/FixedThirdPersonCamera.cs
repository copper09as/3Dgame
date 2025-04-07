using UnityEngine;

public class FixedThirdPersonCamera : MonoBehaviour
{
    public Transform target;             // ��ɫ�������Ҫ����Ķ���
    public Vector3 offset = new Vector3(1.5f, 2.5f, -4f);  // ����ڽ�ɫ��ƫ�ƣ��Һ��Ϸ���
    public float smoothSpeed = 5f;       // ƽ���ƶ��ٶ�

    // FOV����
    public float initialFOV = 60f;      // ��ʼ��Ұ
    public float minFOV = 40f;          // ��С��Ұ
    public float maxFOV = 90f;          // �����Ұ
    public float FOVSpeed = 5f;         // FOVƽ�������ٶ�

    // ����߶ȵ���
    public float heightAdjustmentSpeed = 0.5f; // ��������߶ȵ��ٶ�
    public float minHeightOffset = 1.0f;      // ��С������߶�ƫ�ƣ�����ڽ�ɫ��
    public float maxHeightOffset = 5.0f;      // ��������߶�ƫ�ƣ�����ڽ�ɫ��

    private Camera mainCamera;

    void Start()
    {
        // ��ȡ��������
        mainCamera = Camera.main;
        mainCamera.fieldOfView = initialFOV; // ���ó�ʼ��Ұ��С
    }

    void LateUpdate()
    {
        if (target == null) return;

        // ��������߶ȣ�ͨ����������������ƣ�
        HandleCameraHeight();

        // ����Ŀ��λ��
        Vector3 desiredPosition = target.position + target.right * offset.x + target.up * offset.y + target.forward * offset.z;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // ���������λ��
        transform.position = smoothedPosition;

        // ���ʼ�տ���Ŀ��
        transform.LookAt(target.position + Vector3.up * 1.5f);  // ������޸����������õض�׼��ɫ��ͷ����������λ

        // ����FOV
        HandleFOV();
    }

    void HandleFOV()
    {
        // ��ȡ����������������FOV
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // ����й������룬����FOV
        if (scrollInput != 0f)
        {
            mainCamera.fieldOfView -= scrollInput * 10f; // �������������Ե���10f�ԼӴ���С�仯����
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, minFOV, maxFOV); // ����FOV����Сֵ�����ֵ֮��
        }
    }

    // ��������߶ȵ���
    void HandleCameraHeight()
    {
        // ��ȡ�û���������������ĸ߶�
        float heightInput = Input.GetAxis("Vertical"); // ʹ�� W �� S ���������¼�ͷ������

        // ���ӻ��������ĸ߶�ƫ��
        offset.y -= heightInput * heightAdjustmentSpeed * Time.deltaTime;

        // ��������߶�ƫ�Ƶķ�Χ
        offset.y = Mathf.Clamp(offset.y, minHeightOffset, maxHeightOffset);
    }
}

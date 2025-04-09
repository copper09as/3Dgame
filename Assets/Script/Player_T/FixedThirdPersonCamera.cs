using UnityEngine;

public class FixedThirdPersonCamera : MonoBehaviour
{
    public Transform target;             // 角色（摄像机要跟随的对象）
    public Vector3 offset = new Vector3(1.5f, 2.5f, -4f);  // 相对于角色的偏移（右后方上方）
    public float smoothSpeed = 5f;       // 平滑移动速度

    // FOV设置
    public float initialFOV = 60f;      // 初始视野
    public float minFOV = 40f;          // 最小视野
    public float maxFOV = 90f;          // 最大视野
    public float FOVSpeed = 5f;         // FOV平滑调整速度

    // 相机高度调整
    public float heightAdjustmentSpeed = 0.5f; // 相机调整高度的速度
    public float minHeightOffset = 1.0f;      // 最小的相机高度偏移（相对于角色）
    public float maxHeightOffset = 5.0f;      // 最大的相机高度偏移（相对于角色）

    private Camera mainCamera;

    void Start()
    {
        // 获取摄像机组件
        mainCamera = Camera.main;
        mainCamera.fieldOfView = initialFOV; // 设置初始视野大小
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 调整相机高度（通过按键或者输入控制）
        HandleCameraHeight();

        // 计算目标位置
        Vector3 desiredPosition = target.position + target.right * offset.x + target.up * offset.y + target.forward * offset.z;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 设置摄像机位置
        transform.position = smoothedPosition;

        // 相机始终看向目标
        transform.LookAt(target.position + Vector3.up * 1.5f);  // 你可以修改这里来更好地对准角色的头部或其他部位

        // 调整FOV
        HandleFOV();
    }

    void HandleFOV()
    {
        // 获取鼠标滚轮输入来调整FOV
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // 如果有滚轮输入，调整FOV
        if (scrollInput != 0f)
        {
            mainCamera.fieldOfView -= scrollInput * 10f; // 调整步长，可以调整10f以加大或减小变化幅度
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, minFOV, maxFOV); // 限制FOV在最小值和最大值之间
        }
    }

    // 处理相机高度调整
    void HandleCameraHeight()
    {
        // 获取用户输入来调整相机的高度
        float heightInput = Input.GetAxis("Vertical"); // 使用 W 和 S 键或者上下箭头来调整

        // 增加或减少相机的高度偏移
        offset.y -= heightInput * heightAdjustmentSpeed * Time.deltaTime;

        // 限制相机高度偏移的范围
        offset.y = Mathf.Clamp(offset.y, minHeightOffset, maxHeightOffset);
    }
}

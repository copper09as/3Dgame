using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float floatHeight = 0.3f;  // 物体浮动的高度（微小上下起伏）
    public float floatSpeed = 0.5f;   // 浮动的速度（上下起伏）
    public float waterLevelOffset = 0.1f;  // 水面偏移量，用于保持物体在水面上方

    private bool isInWater = false;   // 物体是否进入水面
    private Rigidbody rb;             // 物体的刚体

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // 获取物体的刚体组件
        rb.useGravity = true;            // 默认启用重力
    }

    void Update()
    {
        if (isInWater)
        {
            // 停用重力以避免物体向下掉
            rb.useGravity = false;

            // 计算浮动的目标位置（轻微上下起伏）
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            Vector3 targetPosition = new Vector3(transform.position.x, waterLevelOffset + yOffset, transform.position.z);

            // 使用刚体的 MovePosition 来平滑移动物体，保持物体在水面上方
            rb.MovePosition(targetPosition);
        }
        else
        {
            // 恢复物体的重力
            rb.useGravity = true;
        }
    }

    // 当物体进入水面时触发
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))  // 水面标签为 "Water"
        {
            isInWater = true;  // 标记物体进入水面
        }
    }

    // 当物体持续在水面时触发
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;  // 物体仍在水面上，持续浮动
        }
    }

    // 当物体离开水面时触发
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;  // 标记物体离开水面
        }
    }
}

using UnityEngine;

public class LubanLockPiece : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool isDragging = false;
    public bool isUnlocked = false; // 标记是否已经解锁

    void Start()
    {
        initialPosition = transform.position;
    }

    // 开始拖动
    public void StartDrag()
    {
        isDragging = true;
        // 可以添加其他拖动开始时的逻辑，比如改变颜色或材质等
    }

    // 处理拖动
    public void TryDrag(Vector3 newPosition)
    {
        if (isDragging)
        {
            transform.position = newPosition; // 拖动到新的位置
        }
    }

    // 结束拖动
    public void EndDrag()
    {
        isDragging = false;
        // 结束拖动后检查是否放置到正确位置
        if (IsInCorrectPosition())
        {
            isUnlocked = true;
        }
        else
        {
            // 如果位置错误，可以将其归回原位
            transform.position = initialPosition;
        }
    }

    // 检查是否放置到正确位置
    private bool IsInCorrectPosition()
    {
        // 在这里根据需要判断部件的放置位置是否正确
        // 例如通过距离、角度等来判断是否符合条件
        // 示例：部件与正确位置的距离小于某个值则视为正确
        return Vector3.Distance(transform.position, initialPosition) < 0.1f;
    }

    // 其他必要的逻辑...
}

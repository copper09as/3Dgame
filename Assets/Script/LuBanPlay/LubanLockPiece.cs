using UnityEngine;

public class LubanLockPiece : MonoBehaviour
{
    public int unlockIndex;                  // 第几顺位解锁
    public Vector3 unlockDirection;         // 正确拖动方向（世界坐标）
    public float requiredDistance = 0.3f;   // 拖动多远判定为取出
    public bool isUnlocked = false;

    private Vector3 dragStartPos;
    private Vector3 currentOffset;

    public void StartDrag(Vector3 hitPoint)
    {
        dragStartPos = hitPoint;
        currentOffset = Vector3.zero;
    }

    public bool TryDrag(Vector3 currentPoint, int currentUnlockStep)
    {
        if (isUnlocked || unlockIndex != currentUnlockStep)
        {
            // 不是当前该解锁的块
            return false;
        }

        Vector3 dragVector = currentPoint - dragStartPos;
        float dot = Vector3.Dot(dragVector.normalized, unlockDirection.normalized);

        if (dot > 0.8f) // 判断拖动方向是否接近目标方向（角度<36°）
        {
            currentOffset = Vector3.Project(dragVector, unlockDirection);
            transform.position += currentOffset;
            dragStartPos = currentPoint;

            // 判断是否已经解锁
            if (currentOffset.magnitude > requiredDistance)
            {
                isUnlocked = true;
                return true; // 解锁成功
            }
        }

        return false; // 拖动方向不正确或未达到距离
    }
}

using UnityEngine;

public class LubanLockPiece : MonoBehaviour
{
    public int unlockIndex;                  // �ڼ�˳λ����
    public Vector3 unlockDirection;         // ��ȷ�϶������������꣩
    public float requiredDistance = 0.3f;   // �϶���Զ�ж�Ϊȡ��
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
            // ���ǵ�ǰ�ý����Ŀ�
            return false;
        }

        Vector3 dragVector = currentPoint - dragStartPos;
        float dot = Vector3.Dot(dragVector.normalized, unlockDirection.normalized);

        if (dot > 0.8f) // �ж��϶������Ƿ�ӽ�Ŀ�귽�򣨽Ƕ�<36�㣩
        {
            currentOffset = Vector3.Project(dragVector, unlockDirection);
            transform.position += currentOffset;
            dragStartPos = currentPoint;

            // �ж��Ƿ��Ѿ�����
            if (currentOffset.magnitude > requiredDistance)
            {
                isUnlocked = true;
                return true; // �����ɹ�
            }
        }

        return false; // �϶�������ȷ��δ�ﵽ����
    }
}

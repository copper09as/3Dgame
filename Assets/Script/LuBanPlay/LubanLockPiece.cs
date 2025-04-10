using UnityEngine;

public class LubanLockPiece : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool isDragging = false;
    public bool isUnlocked = false; // ����Ƿ��Ѿ�����

    void Start()
    {
        initialPosition = transform.position;
    }

    // ��ʼ�϶�
    public void StartDrag()
    {
        isDragging = true;
        // ������������϶���ʼʱ���߼�������ı���ɫ����ʵ�
    }

    // �����϶�
    public void TryDrag(Vector3 newPosition)
    {
        if (isDragging)
        {
            transform.position = newPosition; // �϶����µ�λ��
        }
    }

    // �����϶�
    public void EndDrag()
    {
        isDragging = false;
        // �����϶������Ƿ���õ���ȷλ��
        if (IsInCorrectPosition())
        {
            isUnlocked = true;
        }
        else
        {
            // ���λ�ô��󣬿��Խ�����ԭλ
            transform.position = initialPosition;
        }
    }

    // ����Ƿ���õ���ȷλ��
    private bool IsInCorrectPosition()
    {
        // �����������Ҫ�жϲ����ķ���λ���Ƿ���ȷ
        // ����ͨ�����롢�Ƕȵ����ж��Ƿ��������
        // ʾ������������ȷλ�õľ���С��ĳ��ֵ����Ϊ��ȷ
        return Vector3.Distance(transform.position, initialPosition) < 0.1f;
    }

    // ������Ҫ���߼�...
}

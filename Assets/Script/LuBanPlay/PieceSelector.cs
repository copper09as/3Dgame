using UnityEngine;

public class PieceSelector : MonoBehaviour
{
    public DragController dragController;

    void OnMouseDown()
    {
        // �����ĳ��³��������ʱ�����ò�������Ϊ��ǰ�϶�����
        LubanLockPiece piece = GetComponent<LubanLockPiece>();
        if (piece != null)
        {
            dragController.SetCurrentPiece(piece);
        }
    }
}

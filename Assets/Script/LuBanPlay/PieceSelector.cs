using UnityEngine;

public class PieceSelector : MonoBehaviour
{
    public DragController dragController;

    void OnMouseDown()
    {
        // 当点击某个鲁班锁部件时，将该部件设置为当前拖动部件
        LubanLockPiece piece = GetComponent<LubanLockPiece>();
        if (piece != null)
        {
            dragController.SetCurrentPiece(piece);
        }
    }
}

using UnityEngine;

public class DragController : MonoBehaviour
{
    private LubanLockPiece currentPiece; // 当前拖动的部件

    void Update()
    {
        if (currentPiece != null)
        {
            if (Input.GetMouseButtonDown(0)) // 按下鼠标左键
            {
                currentPiece.StartDrag(); // 开始拖动
            }

            if (Input.GetMouseButton(0)) // 鼠标左键按住
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0; // 确保 z 轴不变
                currentPiece.TryDrag(mousePosition); // 尝试拖动
            }

            if (Input.GetMouseButtonUp(0)) // 鼠标左键松开
            {
                currentPiece.EndDrag(); // 结束拖动
                currentPiece = null; // 清空当前拖动的部件
            }
        }
    }

    public void SetCurrentPiece(LubanLockPiece piece)
    {
        currentPiece = piece;
    }
}

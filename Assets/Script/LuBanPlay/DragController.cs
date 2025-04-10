using UnityEngine;

public class DragController : MonoBehaviour
{
    private Camera cam;
    private LubanLockPiece selectedPiece;
    private int currentUnlockStep = 0; // 当前应解锁第几块

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var piece = hit.collider.GetComponent<LubanLockPiece>();
                if (piece != null)
                {
                    selectedPiece = piece;
                    selectedPiece.StartDrag(hit.point);
                    HighlightPiece(selectedPiece, true);
                }
            }
        }

        if (Input.GetMouseButton(0) && selectedPiece != null)
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                bool unlocked = selectedPiece.TryDrag(hit.point, currentUnlockStep);
                if (unlocked)
                {
                    currentUnlockStep++;
                    HighlightPiece(selectedPiece, false);
                    selectedPiece = null;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedPiece != null)
        {
            HighlightPiece(selectedPiece, false);
            selectedPiece = null;
        }
    }

    void HighlightPiece(LubanLockPiece piece, bool highlight)
    {
        var renderer = piece.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = highlight ? Color.yellow : Color.white;
        }
    }
}

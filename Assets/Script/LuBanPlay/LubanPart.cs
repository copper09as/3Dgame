using UnityEngine;
using System.Collections;

public class LubanPart : MonoBehaviour
{
    public int unlockOrder = 1;
    public Vector3 correctDirection = Vector3.up;
    public float unlockDistance = 1.5f;
    public bool isUnlocked = false;
    public float additionalDistance = 0.5f;

    private Vector3 startMousePos;
    private Vector3 startObjPos;
    private bool isDragging = false;
    private static int currentUnlockOrder = 1;
    private Renderer rend;
    private Color originalColor;
    private float moveSpeed = 20f;
    public Vector3 originalPosition;

    public System.Action OnReturnedToOrigin;

    void Start()
    {
        originalPosition = transform.localPosition; // 修改为本地坐标
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    void OnMouseDown()
    {
        if (isUnlocked) return;
        startMousePos = Input.mousePosition;
        startObjPos = transform.position;
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (!isDragging || isUnlocked) return;

        if (unlockOrder != currentUnlockOrder)
        {
            StartCoroutine(ShakeFeedback());
            ShowErrorColor();
            return;
        }

        Vector3 delta = Input.mousePosition - startMousePos;
        Vector3 worldDelta = Camera.main.ScreenToWorldPoint(new Vector3(delta.x, delta.y, 10f)) -
                           Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10f));

        float dot = Vector3.Dot(worldDelta.normalized, correctDirection);
        if (dot < 0.5f)
        {
            StartCoroutine(ShakeFeedback());
            ShowErrorColor();
            return;
        }

        float moveDistance = Vector3.Project(worldDelta, correctDirection).magnitude;

        if (moveDistance >= unlockDistance)
        {
            isUnlocked = true;
            currentUnlockOrder++;
            ResetColor();
            StartCoroutine(MoveInDirection(correctDirection, unlockDistance + additionalDistance));
            if (unlockOrder == 5)
                LubanController.Instance.OnPuzzleCompleted();
        }
        else
        {
            transform.position = startObjPos + Vector3.Project(worldDelta, correctDirection);
            ResetColor();
        }
    }

    void OnMouseUp()
    {
        if (!isUnlocked)
        {
            transform.position = startObjPos;
            ResetColor();
        }
        isDragging = false;
    }

    IEnumerator ShakeFeedback()
    {
        Vector3 originalPos = transform.position;
        float t = 0f;
        while (t < 0.2f)
        {
            transform.position = originalPos + Random.insideUnitSphere * 0.02f;
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPos;
    }

    IEnumerator MoveInDirection(Vector3 direction, float targetDistance)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + direction.normalized * targetDistance;
        float journeyLength = Vector3.Distance(startPos, targetPos);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
            yield return null;
        }
        transform.position = targetPos;
    }

    public void StartReturnToOrigin()
    {
        StartCoroutine(ReturnToOrigin());
    }

    IEnumerator ReturnToOrigin()
    {
        Vector3 startPos = transform.localPosition;
        float time = 0;
        float duration = 1.2f;

        // 先执行 Lerp 动画
        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPos, originalPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // 动画结束后，强制修正位置（避免浮点误差）
        transform.localPosition = originalPosition;
        OnReturnedToOrigin?.Invoke();
    }

    public void ForceReturnToOrigin()
    {
        transform.localPosition = originalPosition;

    }

    public static void ResetLock()
    {
        currentUnlockOrder = 1;
    }

    void ShowErrorColor() => rend.material.color = Color.red;
    void ResetColor() => rend.material.color = originalColor;
}
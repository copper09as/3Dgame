using UnityEngine;

public class LubanPart : MonoBehaviour
{
    public int unlockOrder = 1;                    // ����˳��
    public Vector3 correctDirection = Vector3.up;  // ��ȷ�϶����򣨵�λ������
    public float unlockDistance = 1.5f;            // �϶���������
    public bool isUnlocked = false;

    private Vector3 startMousePos;
    private Vector3 startObjPos;
    private bool isDragging = false;

    private static int currentUnlockOrder = 1;     // ��ǰ����˳�򣨾�̬��

    private Renderer rend;
    private Color originalColor;
    public float additionalDistance = 0.5f;      // ����������ƶ��ľ���
    private float moveSpeed = 5f;                 // ƽ���ƶ����ٶ�

    void Start()
    {
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

        Vector3 delta = Input.mousePosition - startMousePos;
        Vector3 worldDelta = Camera.main.ScreenToWorldPoint(new Vector3(delta.x, delta.y, 10f)) - Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10f));

        float dot = Vector3.Dot(worldDelta.normalized, correctDirection);
        float tolerance = 0.5f; // �ݴ�Ƕ�

        if (dot < tolerance)
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
            // ƽ���ƶ���������ȷ����������һЩ
            StartCoroutine(MoveInDirection(correctDirection, unlockDistance + additionalDistance));
            Debug.Log("���������ɹ�: " + gameObject.name);
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

    System.Collections.IEnumerator ShakeFeedback()
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

    void ShowErrorColor()
    {
        if (rend != null)
            rend.material.color = Color.red;
    }

    void ResetColor()
    {
        if (rend != null)
            rend.material.color = originalColor;
    }

    // ������ȷ����ƽ���ƶ�
    System.Collections.IEnumerator MoveInDirection(Vector3 direction, float targetDistance)
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

        transform.position = targetPos;  // ȷ�����ȷ��Ŀ��λ��
    }
}

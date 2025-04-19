using UnityEngine;
using System.Linq;
using System.Collections;

public class LubanController : MonoBehaviour
{
    public static LubanController Instance;
    public LubanPart lubanPart;
    public GameObject successPanel;
    public LubanRotator rotator;
    private bool isShowing = false;

    void Start()
    {
        LubanPart.ResetLock(); // 游戏开始时重置顺序
        Instance = this;
    }
    void Awake()
    {
        Instance = this;
    }

    public void OnPuzzleCompleted()
    {
        if (isShowing) return;
        isShowing = true;

        StartCoroutine(ResetAndShowWithFinalCheck());
    }

    IEnumerator ResetAndShowWithFinalCheck()
    {
        LubanPart[] parts = FindObjectsOfType<LubanPart>();
        int completedCount = 0;
        int totalToReset = parts.Count(p => p.isUnlocked);

        // 先让所有部件执行归位动画
        foreach (var part in parts)
        {
            if (part.isUnlocked)
            {
                part.OnReturnedToOrigin += () => completedCount++;
                part.StartReturnToOrigin();
            }
        }

        // 等待所有动画完成
        yield return new WaitUntil(() => completedCount >= totalToReset);

        // 最终强制修正（避免个别部件仍有微小偏差）
        foreach (var part in parts)
        {
            if (part.isUnlocked)
                part.transform.localPosition = part.originalPosition;
        }
        void ShowSuccessPanel()
        {
            if (successPanel != null)
                successPanel.SetActive(true);
        }
        // 显示 UI 和旋转
        ShowSuccessPanel();
        if (rotator != null)
            rotator.StartShowcaseRotation();
    }
}
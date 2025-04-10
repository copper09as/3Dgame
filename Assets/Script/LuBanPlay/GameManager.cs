using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LubanLockPiece[] allPieces; // 所有鲁班锁部件
    private int unlockedCount = 0; // 已解锁的部件数量

    void Start()
    {
        unlockedCount = 0;
    }

    void Update()
    {
        // 检查是否所有部件都已解锁
        CheckGameProgress();
    }

    // 检查所有部件是否解锁
    void CheckGameProgress()
    {
        unlockedCount = 0;
        foreach (LubanLockPiece piece in allPieces)
        {
            if (piece.isUnlocked)
            {
                unlockedCount++;
            }
        }

        if (unlockedCount == allPieces.Length)
        {
            // 所有部件都解锁，胜利！
            WinGame();
        }
    }

    void WinGame()
    {
        // 游戏胜利的处理逻辑
        Debug.Log("鲁班锁解开了！");
    }
}

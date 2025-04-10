using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LubanLockPiece[] allPieces; // ����³��������
    private int unlockedCount = 0; // �ѽ����Ĳ�������

    void Start()
    {
        unlockedCount = 0;
    }

    void Update()
    {
        // ����Ƿ����в������ѽ���
        CheckGameProgress();
    }

    // ������в����Ƿ����
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
            // ���в�����������ʤ����
            WinGame();
        }
    }

    void WinGame()
    {
        // ��Ϸʤ���Ĵ����߼�
        Debug.Log("³�����⿪�ˣ�");
    }
}

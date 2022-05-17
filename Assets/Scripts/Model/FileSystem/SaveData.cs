using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [SerializeField] private int lastPackNum; // save player's last played pack number
    public int LastPackNum { get => lastPackNum; set => lastPackNum = value; }
    [SerializeField] private int lastStageNum; // save player's last played stage number
    public int LastStageNum { get => lastStageNum; set => lastStageNum = value; }
    [SerializeField] private int[] clearStatus;
    public int[] Status => clearStatus;

    public SaveData()
    {
        lastPackNum = 1;
        lastStageNum = 1;
        clearStatus = new int[GameManager.MAX_LEVEL_COUNT * GameManager.MAX_STAGE_COUNT];
    }
    public void AddStatus(int index, int i) => clearStatus[index] = i;
    public int GetStatus(int index) => clearStatus[index];

    public void AddStatus(int level, int stage, int i) => clearStatus[(level - 1) * GameManager.MAX_STAGE_COUNT + stage - 1] = i;
    public int GetStatus(int level, int stage) => clearStatus[(level - 1) * GameManager.MAX_STAGE_COUNT + stage - 1];
}

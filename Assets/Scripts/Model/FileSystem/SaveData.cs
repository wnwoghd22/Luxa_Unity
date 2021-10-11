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
    [SerializeField] private Dictionary<string, int> clearStatus;

    public SaveData()
    {
        lastPackNum = 1;
        lastStageNum = 1;
        clearStatus = new Dictionary<string, int>();
    }

    public void AddStatus(string s, int i) => clearStatus.Add(s, i);
    public int getStatus(string s)
    {
        if (clearStatus.ContainsKey(s))
            return clearStatus[s];
        else
            return 0;
    }

}

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
    private Dictionary<string, int> clearStatus;
    public Dictionary<string, int> Status => clearStatus;
    [SerializeField] private List<string> keyList;
    [SerializeField] private List<int> valueList;

    public SaveData()
    {
        lastPackNum = 1;
        lastStageNum = 1;
        clearStatus = new Dictionary<string, int>();
        keyList = new List<string>();
        valueList = new List<int>();
    }

    public void AddStatus(string s, int i)
    {
        if (clearStatus.ContainsKey(s))
            clearStatus[s] = i;
        else
            clearStatus.Add(s, i);

    }
    public int getStatus(string s)
    {
        if (clearStatus.ContainsKey(s))
            return clearStatus[s];
        else
            return 0;
    }

    public void ConstructDict()
    {
        for (int i = 0; i < keyList.Count; ++i)
            AddStatus(keyList[i], valueList[i]);
    }
    public void DestructDict()
    {
        keyList = new List<string>(clearStatus.Keys);
        valueList = new List<int>(clearStatus.Values);
    }
}

using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

public class FileManager : MonoBehaviour
{
    public async UniTask<string> GetStageFileAsync(int level, int n)
    {
        string filePath = Application.streamingAssetsPath + "/Stages/" + level + "/" + n + ".txt";
#if UNITY_EDITOR
        Debug.Log(filePath);
#endif

        string txt = (await UnityWebRequest.Get(filePath).SendWebRequest()).downloadHandler.text;
        return txt;
    }

    public SaveData ReadSaveFile()
    {
        string filePath = Application.persistentDataPath + "/save.json";
        Debug.Log(filePath);

        if (!File.Exists(filePath))
        {
#if UNITY_EDITOR
            Debug.Log("No file!");
#endif
            return new SaveData();
        }

        StreamReader saveFile = new StreamReader(filePath);

        SaveData data = JsonUtility.FromJson<SaveData>(saveFile.ReadToEnd());

        return data;
    }
    public void WriteSaveFile(SaveData data)
    {
        string filePath = Application.persistentDataPath + "/save.json";

        StreamWriter saveFile = new StreamWriter(filePath);

        saveFile.Write(JsonUtility.ToJson(data));
        saveFile.Close();
    }
}

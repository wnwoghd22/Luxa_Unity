using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class FileManager :MonoBehaviour {
    [SerializeField] private GameManager gm;
    private WaitForFixedUpdate waitForUpdate = new WaitForFixedUpdate();
    private WaitForFixedUpdate wait { get { return waitForUpdate; } }

    private void Start()
    {
        //gm = FindObjectOfType<GameManager>();
    }

    [Obsolete]
    public IEnumerator ReadStageFile(int level,int n)
    {
        RuntimePlatform platform = Application.platform;
        switch(platform)
        {
            case RuntimePlatform.Android:
                Debug.Log("read file for android");
                //result = ReadStageFileForAndroid(n).Result;
                yield return StartCoroutine(ReadStageFileForAndroidCoroutine(level, n));
                break;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                Debug.Log("read file for local");
                ReadStageFileForLocal(level, n);
                break;
        }
    }
    private Board ReadStageFileForLocal(int level, int n) {
        string os = SystemInfo.operatingSystem;

        string filePath = Path.Combine(Application.streamingAssetsPath, "Stages/" + level + "/" + n + ".txt");
        StreamReader file = new StreamReader(filePath);

        string boardSize = file.ReadLine();
        string[] size = boardSize.Split(' ');

        Board b = new Board(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]), Convert.ToInt32(size[2]));

        int beadNum = Convert.ToInt32(file.ReadLine());
        while(beadNum-- > 0) {
            string[] beadInfo = file.ReadLine().Split(' ');
            b.SetBeads(
                Convert.ToInt32(beadInfo[0]),
                Convert.ToInt32(beadInfo[1]),
                Convert.ToInt32(beadInfo[2]),
                Convert.ToInt32(beadInfo[3]),
                Convert.ToInt32(beadInfo[4])
            );
        }

        int ringNum = Convert.ToInt32(file.ReadLine());
        for (int i = 0; i < ringNum; ++i)
        {
            string[] ringInfo = file.ReadLine().Split(' ');
            b.AddRing(
                Convert.ToInt32(ringInfo[0]),
                Convert.ToInt32(ringInfo[1]),
                Convert.ToInt32(ringInfo[2]),
                i
            );
        }

        gm.SetBoard(b);

        return b;
    }
    [Obsolete]
    private IEnumerator ReadStageFileForAndroidCoroutine(int level, int n)
    {
        string filePath = "jar:file://" + Application.dataPath + "!/assets/Stages/" + level + "/" + n + ".txt";
        using var request = UnityWebRequest.Get(filePath);
        var operation = request.SendWebRequest();
        while (!operation.isDone)yield return null;

        string[] fileString = request.downloadHandler.text.Split('\n');
        int idx = 0;

        string[] size = fileString[idx++].Split(' ');

        Board b = new Board(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]), Convert.ToInt32(size[2]));

        int beadNum = Convert.ToInt32(fileString[idx++]);
        while (beadNum-- > 0)
        {
            string[] beadInfo = fileString[idx++].Split(' ');
            b.SetBeads(
                Convert.ToInt32(beadInfo[0]),
                Convert.ToInt32(beadInfo[1]),
                Convert.ToInt32(beadInfo[2]),
                Convert.ToInt32(beadInfo[3]),
                Convert.ToInt32(beadInfo[4])
            );
        }

        int ringNum = Convert.ToInt32(fileString[idx++]);
        for (int i = 0; i < ringNum; ++i)
        {
            string[] ringInfo = fileString[idx++].Split(' ');
            b.AddRing(
                Convert.ToInt32(ringInfo[0]),
                Convert.ToInt32(ringInfo[1]),
                Convert.ToInt32(ringInfo[2]),
                i
            );
        }

        gm.SetBoard(b);

        yield return b;
    }
    public static async UniTask<string> GetStageFileAsync(int level, int n)
    {
        string filePath = Application.streamingAssetsPath + "assets/Stages/ " + level + "/" + n + ".txt";

        string txt = (await UnityWebRequest.Get(filePath).SendWebRequest()).downloadHandler.text;
        return txt;
    }

    [Obsolete]
    private async Task<Board> ReadStageFileForAndroid(int n) {
        Board b = null;

        string filePath = "jar:file://" + Application.dataPath + "!/assets/Stages/" + n + ".txt";
        using var request = UnityWebRequest.Get(filePath);
        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            Debug.Log("not yet");
            await Task.Yield();
        }

        Debug.Log("Done");

        //Debug.Log(Encoding.Default.GetString(request.downloadHandler.data));
        string[] fileString = Encoding.Default.GetString(request.downloadHandler.data).Split('\n');
        int idx = 0;

        //Debug.Log("FileManager.cs 76 : " + fileString.Length);
        for (int i = 0; i < fileString.Length; ++i)
        {
            //Debug.Assert(true);
            Debug.Log(i + ", " + fileString[i]);
        }

        string[] size = fileString[idx++].Split(' ');

        b = new Board(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]), Convert.ToInt32(size[2]));

        int beadNum = Convert.ToInt32(fileString[idx++]);
        while (beadNum-- > 0)
        {
            string[] beadInfo = fileString[idx++].Split(' ');
            b.SetBeads(
                Convert.ToInt32(beadInfo[0]),
                Convert.ToInt32(beadInfo[1]),
                Convert.ToInt32(beadInfo[2]),
                Convert.ToInt32(beadInfo[3]),
                Convert.ToInt32(beadInfo[4])
            );
        }

        int ringNum = Convert.ToInt32(fileString[idx++]);
        for (int i = 0; i < ringNum; ++i)
        {
            string[] ringInfo = fileString[idx++].Split(' ');
            b.AddRing(
                Convert.ToInt32(ringInfo[0]),
                Convert.ToInt32(ringInfo[1]),
                Convert.ToInt32(ringInfo[2]),
                i
            );
        }

        return b;
    } 

    public void ReadSaveFile()
    {
        RuntimePlatform platform = Application.platform;
        switch (platform)
        {
            case RuntimePlatform.Android:
                Debug.Log("read file for android");
                gm.SetData(ReadSaveFileForLocal());
                break;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                Debug.Log("read file for local");
                gm.SetData(ReadSaveFileForLocal());
                break;
        }
    }
    public void WriteSaveFile()
    {
        RuntimePlatform platform = Application.platform;
        switch (platform)
        {
            case RuntimePlatform.Android:
                Debug.Log("write file for android");
                WriteSaveFileForLocal(gm.Data);
                break;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                Debug.Log("write file for local");
                WriteSaveFileForLocal(gm.Data);
                break;
        }
    }
    private SaveData ReadSaveFileForLocal()
    {
        string filePath = Application.persistentDataPath + "/save.json";
        Debug.Log(filePath);

        if (!File.Exists(filePath))
        {
            Debug.Log("No file!");
            return null;
        }

        StreamReader saveFile = new StreamReader(filePath);

        SaveData data = JsonUtility.FromJson<SaveData>(saveFile.ReadToEnd());

        Debug.Log(data);

        if (data != null)
        {
            Debug.Log(data.LastPackNum + " " + data.LastStageNum);
            data.ConstructDict();
        }

        return data;
    }
    private void WriteSaveFileForLocal(SaveData data)
    {
        string filePath = Application.persistentDataPath + "/save.json";

        StreamWriter saveFile = new StreamWriter(filePath);

        data.DestructDict();

        saveFile.Write(JsonUtility.ToJson(data));
        saveFile.Close();
    }
}

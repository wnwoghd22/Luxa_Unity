using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class FileManager :MonoBehaviour {
    private GameManager gm;
    private WaitForFixedUpdate waitForUpdate = new WaitForFixedUpdate();
    private WaitForFixedUpdate wait { get { return waitForUpdate; } }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public IEnumerator ReadStageFile(int n)
    {
        RuntimePlatform platform = Application.platform;
        switch(platform)
        {
            case RuntimePlatform.Android:
                Debug.Log("read file for android");
                //result = ReadStageFileForAndroid(n).Result;
                yield return StartCoroutine(ReadStageFileForAndroidCoroutine(n));
                break;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                Debug.Log("read file for local");
                ReadStageFileForLocal(n);
                break;
        }
    }
    private Board ReadStageFileForLocal(int n) {
        string os = SystemInfo.operatingSystem;

        string filePath = Path.Combine(Application.streamingAssetsPath, "Stages/" + n + ".txt");
        StreamReader file = new StreamReader(filePath);

        string boardSize = file.ReadLine();
        string[] size = boardSize.Split(' ');

        Board b = new Board(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]));

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
    private IEnumerator ReadStageFileForAndroidCoroutine(int n)
    {
        string filePath = "jar:file://" + Application.dataPath + "!/assets/Stages/" + n + ".txt";
        using var request = UnityWebRequest.Get(filePath);
        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            Debug.Log("not yet");
            yield return null;
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

        Board b = new Board(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]));

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

        b = new Board(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]));

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
}

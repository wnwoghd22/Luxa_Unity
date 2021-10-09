using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    FileManager fm = new FileManager();
    UIManager ui;
    Board board;
    Viewer viewer;
    int stageNum;
    int rotateCount;
    List<(int, bool)> playLog;

    private void Awake()
    {
        var obj = FindObjectsOfType<GameManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        viewer = FindObjectOfType<Viewer>();
        ui = FindObjectOfType<UIManager>();

        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == "Title")
        {
            ui.SetTitleUIActive(true);
            ui.SetGameUIActive(false);
            //InitializeStage(1);
        }
        else if (activeScene.name == "GameScene")
        {
            ui.SetTitleUIActive(false);
            ui.SetGameUIActive(true);
            InitializeStage(1);
        }
    }

    void Update()
    {
        
    }
    public void InitializeTitle()
    {
        viewer.CreateTitleBoard();
        stageNum = 1;
        rotateCount = 0;
    }
    public void InitializeStage(int n)
    {
        board = fm.ReadStageFile(n);
        viewer.CreateBoard(board);
        ui.SetStageNum(n);
        stageNum = n;
        rotateCount = 0;
        ui.SetRotateCount("" + 0);
        playLog = new List<(int, bool)>();
    }

    public void BoardUpdate(int index, float zeta)
    {
        if (Mathf.Abs(zeta) < 0.3f)
        {
            viewer.UndoRotate(index);
        }
        else
        {
            ++rotateCount;
            ui.SetRotateCount("" + rotateCount);
            playLog.Add((index, zeta < 0));
            //foreach ((int, bool) item in playLog) Debug.Log(item);
            board.Rotate(index, zeta < 0);
            viewer.UpdateBoard(index, zeta < 0);
        }
    }
    public void SetRingActivate(int i) => viewer.SetRingActivate(i);

    public void Undo()
    {
        if (playLog.Count == 0) return;

        (int, bool) lastMove = playLog[playLog.Count - 1];
        playLog.RemoveAt(playLog.Count - 1);

       --rotateCount;
        ui.SetRotateCount("" + rotateCount);
        board.Rotate(lastMove.Item1, !lastMove.Item2);
        viewer.UpdateBoard(lastMove.Item1, !lastMove.Item2);
    }
}

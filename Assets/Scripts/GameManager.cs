using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    FileManager fm;
    UIManager ui;
    Board board;
    public void SetBoard(Board b) => board = b;
    Viewer viewer;
    int stageNum;
    public int StageNum
    {
        get => stageNum;
        private set => stageNum = value <= 1 ? 1 : value;
    }
    int rotateCount;
    List<(int, bool)> playLog;
    Scene activeScene;

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
        //viewer = FindObjectOfType<Viewer>();
        //ui = FindObjectOfType<UIManager>();

        //activeScene = SceneManager.GetActiveScene();
        //Debug.Log(activeScene.name);
        //if (activeScene.name == "Title")
        //{
        //    ui.SetTitleUIActive(true);
        //    ui.SetGameUIActive(false);
        //    InitializeTitle();
        //}
        //else if (activeScene.name == "GameScene")
        //{
        //    ui.SetTitleUIActive(false);
        //    ui.SetGameUIActive(true);
        //    InitializeStage(StageNum);
        //}
    }

    /* deprecated
    private void OnLevelWasLoaded(int level)
    {
        activeScene = SceneManager.GetActiveScene();
        Debug.Log(activeScene.name);
        if (activeScene.name == "Title")
        {
            ui.SetTitleUIActive(true);
            ui.SetGameUIActive(false);
            InitializeTitle();
        }
        else if (activeScene.name == "GameScene")
        {
            ui.SetTitleUIActive(false);
            ui.SetGameUIActive(true);
            InitializeStage(StageNum);
        }
    } */
    void OnEnable() => SceneManager.sceneLoaded += OnLevelFinishedLoading;
    void OnDisable() => SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Level Loaded");
        //Debug.Log(scene.name);
        //Debug.Log(mode);

        activeScene = scene;
        Debug.Log(activeScene.name);
        if (activeScene.name == "Title")
        {
            if (fm == null)
                fm = FindObjectOfType<FileManager>();
            if (viewer == null)
                viewer = FindObjectOfType<Viewer>();
            if (ui == null)
                ui = FindObjectOfType<UIManager>();
            ui.SetTitleUIActive(true);
            ui.SetGameUIActive(false);
            InitializeTitle();
        }
        else if (activeScene.name == "GameScene")
        {
            ui.SetTitleUIActive(false);
            ui.SetGameUIActive(true);
            StartCoroutine(InitializeStage(StageNum));
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
        ui.SetTitleStageNum(stageNum);
        ui.SetPackNum(1);
    }
    public IEnumerator InitializeStage(int n)
    {
        StartCoroutine(fm.ReadStageFile(n));

        //yield return new WaitUntil(() => board != null);

        //Debug.Log(board.GetBoard().Length);

        while (board == null)
        {
            Debug.Log("wait");
            yield return null;
        }

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
            if (activeScene.name == "Title") UpdateBoardForTitle(index, zeta);
            else if (activeScene.name == "GameScene")UpdateBoardForGameScene(index, zeta);
        }
    }
    public void UpdateBoardForTitle(int index, float zeta)
    {
        if (index == 0)
        {
            //set packNum
        }
        else if (index == 1)
        {
            StageNum += zeta < 0 ? 1 : -1;
            ui.SetTitleStageNum(StageNum);
        }
        viewer.UpdateBoard(index, zeta < 0);
    }
    public void UpdateBoardForGameScene(int index, float zeta)
    {
        ++rotateCount;
        ui.SetRotateCount("" + rotateCount);
        playLog.Add((index, zeta < 0));
        //foreach ((int, bool) item in playLog) Debug.Log(item);
        board.Rotate(index, zeta < 0);
        viewer.UpdateBoard(index, zeta < 0);
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

    public void MoveToGameScene() => SceneManager.LoadScene("GameScene");
    public void BackToTitle() => SceneManager.LoadScene("Title");
}

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
    //int stageNum = 1;
    SaveData data;
    public SaveData Data => data;
    public int StageNum
    {
        get => data.LastStageNum;
        private set => data.LastStageNum = value <= 1 ? 1 : value;
    }
    int rotateCount;
    List<(int, bool)> playLog;
    Scene activeScene;
    bool isComplete
    {
        get
        {
            if (board == null) return false;
            else return board.isComplete;
        }
    }
    WaitUntil isSolved;

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
        isSolved = new WaitUntil(() => isComplete);
    }

    void OnEnable() => SceneManager.sceneLoaded += OnLevelFinishedLoading;
    void OnDisable() => SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        activeScene = scene;
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
    private void InitializeTitle()
    {
        fm.ReadSaveFile();


        viewer.CreateTitleBoard();
        //StageNum = data.LastStageNum;
        
        rotateCount = 0;
        ui.SetTitleStageNum(StageNum);
        ui.SetPackNum(1);
    }
    public void SetData(SaveData data) => this.data = data == null ? new SaveData() : data;

    private IEnumerator InitializeStage(int n)
    {
        board = null;
        ui.SetStageNum(n);
        rotateCount = 0;
        ui.SetRotateCount("" + 0);
        playLog = new List<(int, bool)>();

        fm.WriteSaveFile();

        StartCoroutine(fm.ReadStageFile(n));

        while (board == null) yield return null;

        viewer.CreateBoard(board);
        //stageNum = n;
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
            else if (activeScene.name == "GameScene") UpdateBoardForGameScene(index, zeta);
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
        if (isComplete) return;

        ++rotateCount;
        ui.SetRotateCount("" + rotateCount);
        playLog.Add((index, zeta < 0));
        //foreach ((int, bool) item in playLog) Debug.Log(item);
        board.Rotate(index, zeta < 0);
        viewer.UpdateBoard(index, zeta < 0);

        if (isComplete)
        {
            ui.SetRotateCountWithCheck(rotateCount);
            StartCoroutine(MoveToNextLevelCoroutine());
        }
    }

    public void SetRingActivate(int i)
    {
        if (isComplete) return;
        viewer.SetRingActivate(i);
    }

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

    public void HandleEscapeButton()
    {
        if (activeScene.name == "GameScene") BackToTitle();
        else if (activeScene.name == "Title") Application.Quit();
    }

    public void NextStage() => StartCoroutine(InitializeStage(++StageNum));
    public void PreviousStage() => StartCoroutine(InitializeStage(--StageNum));
    public void RestartStage() => StartCoroutine(InitializeStage(StageNum));

    private IEnumerator MoveToNextLevelCoroutine()
    {
        //effect

        //need to add condition check if player solved problem with minimum rotation
        data.AddStatus(data.LastPackNum + "-" + data.LastStageNum, 1);
        fm.WriteSaveFile();

        yield return new WaitForSeconds(0.3f);

        NextStage();
    }

}

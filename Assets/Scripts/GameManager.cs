using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    FileManager fm;
    UIManager ui;
    PlayGamesServiceManager pm;
    Board board;
    public void SetBoard(Board b) => board = b;
    Viewer viewer;
    //int stageNum = 1;
    SaveData data;
    public SaveData Data => data;
    public int StageNum
    {
        get => data.LastStageNum;
        private set => data.LastStageNum = value <= 1 ? 1 : value >= MAX_STAGE_COUNT ? MAX_STAGE_COUNT : value;
    }
    public int Level
    {
        get => data.LastPackNum;
        private set => data.LastPackNum = value <= 1 ? 1 : value >= MAX_LEVEL_COUNT ? MAX_LEVEL_COUNT : value;
    }

    private const int MAX_STAGE_COUNT = 30;
    private const int MAX_LEVEL_COUNT = 5;

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

    private WaitUntil isSolved;

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
            if (pm == null)
                pm = FindObjectOfType<PlayGamesServiceManager>();
            ui.SetTitleUIActive(true);
            ui.SetGameUIActive(false);
            InitializeTitle();
        }
        else if (activeScene.name == "GameScene")
        {
            ui.SetTitleUIActive(false);
            ui.SetGameUIActive(true);
            StartCoroutine(InitializeStage());
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
        ui.SetPackNum(Level);
    }
    public void SetData(SaveData data) => this.data = data == null ? new SaveData() : data;

    private IEnumerator InitializeStage()
    {
        board = null;
        ui.SetStageNum(Level + " - " + StageNum);
        playLog = new List<(int, bool)>();

        fm.WriteSaveFile();

        StartCoroutine(fm.ReadStageFile(Level, StageNum));

        while (board == null) yield return null;

        viewer.CreateBoard(board);

        rotateCount = 0;
        ui.SetRotateCount(rotateCount, board.Minimum);
    }

    public void UndoRotate(int index)
    {
        viewer.UndoRotate(index);
    }
    public void BoardUpdate(int index, float zeta)
    {
        if (activeScene.name == "Title") UpdateBoardForTitle(index, zeta);
        else if (activeScene.name == "GameScene") UpdateBoardForGameScene(index, zeta);
    }
    public void UpdateBoardForTitle(int index, float zeta)
    {
        if (index == 0)
        {
            Level += zeta < 0 ? 1 : -1;
            ui.SetPackNum(Level);
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
        ui.SetRotateCount(rotateCount, board.Minimum);
        playLog.Add((index, zeta < 0));
        //foreach ((int, bool) item in playLog) Debug.Log(item);
        board.Rotate(index, zeta < 0);
        viewer.UpdateBoard(index, zeta < 0);

        if (isComplete)
        {
            //if player solved problem with minimum rotation
            if (rotateCount == board.Minimum)
                ui.SetRotateCountWithStar(rotateCount, board.Minimum);
            else
                ui.SetRotateCountWithCheck(rotateCount, board.Minimum);
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
        ui.SetRotateCount(rotateCount, board.Minimum);
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

    public void NextStage()
    {
        if (StageNum == MAX_STAGE_COUNT)
        {
            if (Level == MAX_LEVEL_COUNT) return;
            else
            {
                Level += 1;
                StageNum = 1;
            }
        }
        else StageNum += 1;

        StartCoroutine(InitializeStage());
    }
    public void PreviousStage()
    {
        if (StageNum == 1)
        {
            if (Level == 1) return;
            else
            {
                Level -= 1;
                StageNum = MAX_STAGE_COUNT;
            }
        }
        else StageNum -= 1;

        StartCoroutine(InitializeStage());
    }
    public void RestartStage() => StartCoroutine(InitializeStage());

    private IEnumerator MoveToNextLevelCoroutine()
    {
        viewer.ClearEffect();

        data.AddStatus(data.LastPackNum + "-" + data.LastStageNum, rotateCount == board.Minimum ? 2 : 1);
        fm.WriteSaveFile();

        CheckAchievement();

        yield return new WaitForSeconds(1.0f);

        NextStage();
    }

    private void CheckAchievement()
    {
        pm.UnlockAchievement("First Step");
        if (data.Status.ContainsValue(2))
            pm.UnlockAchievement("Perfect Solution");

        bool completeFlag = true, conquerFlag = true;
        for (int i = 1; i <= 30; ++i)
        {
            if (data.Status.ContainsKey(Level + "-" + i))
            {
                int result = data.Status[Level + "-" + i];
                if (result != 2)
                {
                    conquerFlag = false;
                }
            }
            else
            {
                completeFlag = false;
                conquerFlag = false;
                break;
            }
        }
        if (conquerFlag) pm.UnlockAchievement("Conquer Level " + Level);
        if (completeFlag) pm.UnlockAchievement("Complete Level " + Level);
    }
}

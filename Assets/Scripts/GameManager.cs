using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: SerializeField] public FileManager FileManager { get; private set; }
    [field: SerializeField] public UIManager UIManager { get; private set; }
    [field: SerializeField] public PlayGamesServiceManager GPGS { get; private set; }
    [field: SerializeField] public Viewer Viewer { get; private set; }

    Board board;
    public void SetBoard(Board b) => board = b;

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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
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
            UIManager.SetTitleUIActive(true);
            UIManager.SetGameUIActive(false);
            InitializeTitle();
        }
        else if (activeScene.name == "GameScene")
        {
            UIManager.SetTitleUIActive(false);
            UIManager.SetGameUIActive(true);

            UniTask.Create(() => InitStageAsync());
        }
    }

    void Update()
    {

    }
    private void InitializeTitle()
    {
        data = FileManager.ReadSaveFile();


        Viewer.CreateTitleBoard();
        //StageNum = data.LastStageNum;

        rotateCount = 0;
        UIManager.SetTitleStageNum(StageNum);
        UIManager.SetPackNum(Level);
    }
    public void SetData(SaveData data) => this.data = data == null ? new SaveData() : data;

    private async UniTask InitStageAsync()
    {
        UIManager.SetStageNum(Level + " - " + StageNum);
        playLog = new List<(int, bool)>();

        FileManager.WriteSaveFile(Data);

        board = DataParser.ParseBoardData(await FileManager.GetStageFileAsync(Level, StageNum));

        Viewer.CreateBoard(board);

        rotateCount = 0;
        UIManager.SetRotateCount(rotateCount, board.Minimum);
    }

    public void UndoRotate(int index)
    {
        Viewer.UndoRotate(index);
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
            UIManager.SetPackNum(Level);
        }
        else if (index == 1)
        {
            StageNum += zeta < 0 ? 1 : -1;
            UIManager.SetTitleStageNum(StageNum);
        }
        Viewer.UpdateBoard(index, zeta < 0);
    }
    public void UpdateBoardForGameScene(int index, float zeta)
    {
        if (isComplete) return;

        ++rotateCount;
        UIManager.SetRotateCount(rotateCount, board.Minimum);
        playLog.Add((index, zeta < 0));
        //foreach ((int, bool) item in playLog) Debug.Log(item);
        board.Rotate(index, zeta < 0);
        Viewer.UpdateBoard(index, zeta < 0);

        if (isComplete)
        {
            //if player solved problem with minimum rotation
            if (rotateCount == board.Minimum)
                UIManager.SetRotateCountWithStar(rotateCount, board.Minimum);
            else
                UIManager.SetRotateCountWithCheck(rotateCount, board.Minimum);
            // StartCoroutine(MoveToNextLevelCoroutine());
            UniTask.Create(() => MoveToNextLevelAsync());
        }
    }

    public void SetRingActivate(int i)
    {
        if (isComplete) return;
        Viewer.SetRingActivate(i);
    }

    public void Undo()
    {
        if (playLog.Count == 0) return;

        (int, bool) lastMove = playLog[playLog.Count - 1];
        playLog.RemoveAt(playLog.Count - 1);

        --rotateCount;
        UIManager.SetRotateCount(rotateCount, board.Minimum);
        board.Rotate(lastMove.Item1, !lastMove.Item2);
        Viewer.UpdateBoard(lastMove.Item1, !lastMove.Item2);
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

        UniTask.Create(() => InitStageAsync());
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

        UniTask.Create(() => InitStageAsync());
    }
    public async void RestartStage() => await InitStageAsync();

    private async UniTask MoveToNextLevelAsync()
    {
        Viewer.ClearEffect();

        data.AddStatus(data.LastPackNum + "-" + data.LastStageNum, rotateCount == board.Minimum ? 2 : 1);
        FileManager.WriteSaveFile(Data);

        CheckAchievement();

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        NextStage();
    }

    private void CheckAchievement()
    {
        GPGS.UnlockAchievement("First Step");
        if (data.Status.ContainsValue(2))
            GPGS.UnlockAchievement("Perfect Solution");

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
        if (conquerFlag) GPGS.UnlockAchievement("Conquer Level " + Level);
        if (completeFlag) GPGS.UnlockAchievement("Complete Level " + Level);
    }
}

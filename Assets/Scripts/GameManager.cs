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

    public const int MAX_STAGE_COUNT = 30;
    public const int MAX_LEVEL_COUNT = 5;
    public int CurrentStateIndex => (Level - 1) * MAX_STAGE_COUNT + StageNum - 1;

    public const int STATE_NONE = 0;
    public const int STATE_SOLVED = 1;
    public const int STATE_MINIMUM = 2;

    Stack<(int, bool)> playLog;
    int RotateCount => playLog.Count;

    Scene activeScene;
    bool isComplete
    {
        get
        {
            if (board == null) return false;
            else return board.isComplete;
        }
    }

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
        data ??= FileManager.ReadSaveFile();

        Viewer.CreateTitleBoard();
        //StageNum = data.LastStageNum;

        UIManager.SetTitleStageNum(StageNum);
        UIManager.SetPackNum(Level);
    }
    public void SetData(SaveData data) => this.data = data == null ? new SaveData() : data;

    private async UniTask InitStageAsync()
    {
        UIManager.SetStageNum(Level + " - " + StageNum);
        playLog = new Stack<(int, bool)>();

        FileManager.WriteSaveFile(Data);

        board = DataParser.ParseBoardData(await FileManager.GetStageFileAsync(Level, StageNum));

        Viewer.CreateBoard(board);

        UIManager.SetRotateCount(RotateCount, board.Minimum);
    }

    public void UndoRotate(int index)
    {
        Viewer.UndoRotate(index);
    }

    /// <summary>
    /// Should it be like this...?
    /// How about make TitleBoard Class that inherits Board?
    /// </summary>
    /// <param name="index"></param>
    /// <param name="zeta"></param>
    [Obsolete]
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

        playLog.Push((index, zeta < 0));

        UIManager.SetRotateCount(RotateCount, board.Minimum);

        board.Rotate(index, zeta < 0);
        Viewer.UpdateBoard(index, zeta < 0);

        if (isComplete)
        {
            //if player solved problem with minimum rotation
            if (RotateCount == board.Minimum)
                UIManager.SetRotateCountWithStar(RotateCount, board.Minimum);
            else
                UIManager.SetRotateCountWithCheck(RotateCount, board.Minimum);
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

        (int, bool) lastMove = playLog.Pop();

        UIManager.SetRotateCount(RotateCount, board.Minimum);
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
        Data.AddStatus(CurrentStateIndex, RotateCount == board.Minimum ? STATE_MINIMUM : STATE_SOLVED);
        FileManager.WriteSaveFile(Data);

        CheckAchievement();
        
        await Viewer.ClearEffectAsync();

        NextStage();
    }

    private void CheckAchievement()
    {
        GPGS.UnlockAchievement(GPGSIds.achievement_first_step);
        if (RotateCount == board.Minimum)
            GPGS.UnlockAchievement(GPGSIds.achievement_perfect_solution);

        int state = 2;
        for (int i = CurrentStateIndex; i < Level * MAX_STAGE_COUNT; ++i)
            state = Math.Min(state, Data.Status[i]);

        switch (state)
        {
            case STATE_SOLVED:
                GPGS.UnlockAchievement(GPGS.CompleteKeyList[Level - 1]);
                break;
            case STATE_MINIMUM:
                GPGS.UnlockAchievement(GPGS.ConquerKeyList[Level - 1]);
                break;
            case STATE_NONE:
            default:
                break;
        }
    }
}

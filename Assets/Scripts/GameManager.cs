using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    FileManager fm = new FileManager();
    Board board;
    Viewer viewer;

    void Start()
    {
        viewer = FindObjectOfType<Viewer>();
        InitializeStage(1);
    }

    void Update() {
        
    }

    public void InitializeStage(int n)
    {
        board = fm.ReadStageFileForLocal(n);
        viewer.CreateBoard(board);
    }

    public void ViewerUpdate(int index, bool dir)
    {
        board.Rotate(index, dir);
        viewer.UpdateBoard(index, dir);
    }
    public void SetRingActivate(int i) => viewer.SetRingActivate(i);
}

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
        //InitializeStage(7);
    }

    void Update()
    {
        
    }

    public void InitializeStage(int n)
    {
        board = fm.ReadStageFileForLocal(n);
        viewer.CreateBoard(board);
    }

    public void ViewerUpdate(int index, float zeta)
    {
        if (Mathf.Abs(zeta) < 0.3f)
        {
            viewer.UndoRotate(index);
        }
        else
        {
            board.Rotate(index, zeta < 0);
            viewer.UpdateBoard(index, zeta < 0);
        }
    }
    public void SetRingActivate(int i) => viewer.SetRingActivate(i);
}

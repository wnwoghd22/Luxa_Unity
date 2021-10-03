using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    FileManager fm = new FileManager();
    Board board;

    void Start() {
        board = fm.ReadStageFileForLocal(1);
    }

    void Update() {
        
    }
}

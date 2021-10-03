using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class FileManager {
   
    public Board ReadStageFileForLocal(int n) {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Stages/" + n + ".txt");
        StreamReader file = new StreamReader(filePath);

        string boardSize = file.ReadLine();
        string[] size = boardSize.Split(' ');

        Board b = new Board(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]));

        Debug.Log(size[0] + size[1]);

        return b;
    }
    public Board ReadStageFileForAndroid(int n) {
        //string filePath = Path.Combine(Application.streamingAssetsPath, "Stages/" + n + ".txt");
        //StreamReader file = new StreamReader(filePath);

        //string boardSize = file.ReadLine();
        //string[] size = boardSize.Split(' ');

        //Board b = new Board(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]));

        //Debug.Log(size[0] + size[1]);

        //return b;

        return null;
    } 
}

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

        int beadNum = Convert.ToInt32(file.ReadLine());
        while(beadNum-- > 0) {
            string[] beadInfo = file.ReadLine().Split(' ');
            b.SetBeads(
                Convert.ToInt32(beadInfo[0]),
                Convert.ToInt32(beadInfo[1]),
                Convert.ToInt32(beadInfo[2]),
                Convert.ToInt32(beadInfo[3]),
                Convert.ToInt32(beadInfo[4])
            );
        }

        int ringNum = Convert.ToInt32(file.ReadLine());
        for (int i = 0; i < ringNum; ++i)
        {
            string[] ringInfo = file.ReadLine().Split(' ');
            b.AddRing(
                Convert.ToInt32(ringInfo[0]),
                Convert.ToInt32(ringInfo[1]),
                Convert.ToInt32(ringInfo[2]),
                i
            );
        }

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

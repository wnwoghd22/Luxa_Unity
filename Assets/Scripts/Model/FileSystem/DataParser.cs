using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataParser
{
    public static Board ParseBoardData(string data)
    {
        string[] fileString = data.Split('\n');
        int idx = 0;

        string[] size = fileString[idx++].Split(' ');

        Board b = new Board(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]), Convert.ToInt32(size[2]));

        int beadNum = Convert.ToInt32(fileString[idx++]);
        while (beadNum-- > 0)
        {
            string[] beadInfo = fileString[idx++].Split(' ');
            b.SetBeads(
                Convert.ToInt32(beadInfo[0]),
                Convert.ToInt32(beadInfo[1]),
                Convert.ToInt32(beadInfo[2]),
                Convert.ToInt32(beadInfo[3]),
                Convert.ToInt32(beadInfo[4])
            );
        }

        int ringNum = Convert.ToInt32(fileString[idx++]);
        for (int i = 0; i < ringNum; ++i)
        {
            string[] ringInfo = fileString[idx++].Split(' ');
            b.AddRing(
                Convert.ToInt32(ringInfo[0]),
                Convert.ToInt32(ringInfo[1]),
                Convert.ToInt32(ringInfo[2]),
                i
            );
        }

        return b;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_read : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FileManager fm = new FileManager();
        fm.ReadStageFileForLocal(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text stageNum;
    [SerializeField]
    Text rotateCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStageNum(int n)
    {
        stageNum.text = "" + n;
    }
    public void SetStageNum(string s)
    {
        stageNum.text = s;
    }
    public void SetRotateCount(string s)
    {
        rotateCount.text = s;
    }
}

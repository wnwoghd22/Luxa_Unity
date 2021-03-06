using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject titleUI;
    [SerializeField]
    GameObject gameUI;

    [SerializeField]
    Text titleStageNum;
    [SerializeField]
    Text packNum;

    [SerializeField]
    Text homeButton;
    [SerializeField]
    Text undoButton;

    [SerializeField]
    Text stageNum;
    [SerializeField]
    Text rotateCount;

    // Start is called before the first frame update
    void Start()
    {
        homeButton.text = "" + '\u2302';
        undoButton.text = "" + '\u21BA';
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTitleUIActive(bool b) => titleUI.SetActive(b);
    public void SetGameUIActive(bool b) => gameUI.SetActive(b);

    public void SetTitleStageNum(int n) => titleStageNum.text = "" + n;
    public void SetPackNum(int n) => packNum.text = "" + n;

    public void SetStageNum(int n) => stageNum.text = "" + n;
    public void SetStageNum(string s) => stageNum.text = s;

    public void SetRotateCount(int count, int minimum) => rotateCount.text = count + " (" + minimum + ")";
    public void SetRotateCountWithCheck(int count, int minimum) => rotateCount.text = count + " (" + minimum + ")" + '\u2713';
    public void SetRotateCountWithStar(int count, int minimum) => rotateCount.text = count + " (" + minimum + ")" + '\u2605';
}

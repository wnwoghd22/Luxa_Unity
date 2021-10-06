using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardObject : MonoBehaviour
{
    float offsetX;
    float offsetY;
    float unit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SetGridPos(float offsetX, float offsetY, float unit, int x, int y, int z = 0)
    {
        this.offsetX = offsetX;
        this.offsetY = offsetY;
        this.unit = unit;
        float pos_x = offsetX + (((x + 1) % 2) + y * 2) * unit / 2;
        float pos_y = offsetY - x * Mathf.Sqrt(3) * unit / 2;
        Vector3 pos = new Vector3(pos_x, pos_y, 0);

        gameObject.transform.position = pos;
        Vector3 originScale = gameObject.transform.localScale;
        //Debug.Log(unit);
        gameObject.transform.localScale = new Vector3(originScale.x * unit / 2, originScale.y * unit / 2, 1);

    }
    public virtual void SetGridPos(int x, int y, int z = 0)
    {
        float pos_x = offsetX + (((x + 1) % 2) + y * 2) * unit / 2;
        float pos_y = offsetY - x * Mathf.Sqrt(3) * unit / 2;
        Vector3 pos = new Vector3(pos_x, pos_y, 0);

        gameObject.transform.position = pos;
    }
}

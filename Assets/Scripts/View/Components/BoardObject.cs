using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SetGridPos(int x, int y, int z = 0) {
        float pos_x = ((x + 1) % 2) + y * 2;
        float pos_y = 3 - x * Mathf.Sqrt(3);
        Vector3 pos = new Vector3(pos_x, pos_y, 0);

        gameObject.transform.position = pos;
    }
}

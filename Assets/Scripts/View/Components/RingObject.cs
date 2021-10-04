using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingObject : BoardObject
{
    public int posX { get; private set; }
    public int posY { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SetGridPos(int x, int y)
    {
        float pos_x = ((x + 1) % 2) + y * 2;
        float pos_y = 3 - x * Mathf.Sqrt(3);
        Vector3 pos = new Vector3(pos_x, pos_y, 1);

        gameObject.transform.position = pos;
    }

    public void SetPos(int x, int y) { posX = x; posY = y; }
    public void SetColor(int _color)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(_color == 0 ? 1 : 0, _color == 1 ? 1 : 0, _color == 2 ? 1 : 0, 0.3f);
    }
}

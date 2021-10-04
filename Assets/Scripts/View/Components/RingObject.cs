using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingObject : BoardObject
{
    private int x;
    public int posX => x;
    private int y;
    public int posY => y;
    public int Index { get; private set; }
    

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
        SetPos(x, y);
        float pos_x = ((x + 1) % 2) + y * 2;
        float pos_y = 3 - x * Mathf.Sqrt(3);
        Vector3 pos = new Vector3(pos_x, pos_y, 1);

        gameObject.transform.position = pos;
    }

    public void SetIndex(int i) => Index = i;
    public void SetPos(int x, int y) { this.x = x; this.y = y; }
    public void SetColor(int _color)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(_color == 0 ? 1 : 0, _color == 1 ? 1 : 0, _color == 2 ? 1 : 0, 0.3f);
    }
    public void SetAlpha(float f)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        Color currentColor = renderer.color;
        renderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, f);
    }

    public void RotateClockwise(BeadObject[,] board)
    {
        Debug.Log("rotate clockwise");

        if (posX % 2 == 0)
        { // is even
            BeadObject temp = board[x - 1, y];
            board[x - 1, y] = board[x, y - 1];
            board[x, y - 1] = board[x + 1, y];
            board[x + 1, y] = board[x + 1, y + 1];
            board[x + 1, y + 1] = board[x, y + 1];
            board[x, y + 1] = board[x - 1, y + 1];
            board[x - 1, y + 1] = temp;

            board[x - 1, y].SetGridPos(x - 1, y);
            board[x, y - 1].SetGridPos(x, y - 1);
            board[x + 1, y].SetGridPos(x + 1, y);
            board[x + 1, y + 1].SetGridPos(x + 1, y + 1);
            board[x, y + 1].SetGridPos(x, y + 1);
            board[x - 1, y + 1].SetGridPos(x - 1, y + 1);
        }
        else
        { // is odd
            BeadObject temp = board[x - 1, y - 1];
            board[x - 1, y - 1] = board[x, y - 1];
            board[x, y - 1] = board[x + 1, y - 1];
            board[x + 1, y - 1] = board[x + 1, y];
            board[x + 1, y] = board[x, y + 1];
            board[x, y + 1] = board[x - 1, y];
            board[x - 1, y] = temp;

            board[x - 1, y - 1].SetGridPos(x - 1, y - 1);
            board[x, y - 1].SetGridPos(x, y - 1);
            board[x + 1, y - 1].SetGridPos(x + 1, y - 1);
            board[x + 1, y].SetGridPos(x + 1, y);
            board[x, y + 1].SetGridPos(x, y + 1);
            board[x - 1, y].SetGridPos(x - 1, y);
        }
    }
    public void RotateCounterclockwise(BeadObject[,] board)
    {
        if (posX % 2 == 0)
        { // is even
            BeadObject temp = board[x - 1, y];
            board[x - 1, y] = board[x - 1, y + 1];
            board[x - 1, y + 1] = board[x, y + 1];
            board[x, y + 1] = board[x + 1, y + 1];
            board[x + 1, y + 1] = board[x + 1, y];
            board[x + 1, y] = board[x, y - 1];
            board[x, y - 1] = temp;

            board[x - 1, y].SetGridPos(x - 1, y);
            board[x, y - 1].SetGridPos(x, y - 1);
            board[x + 1, y].SetGridPos(x + 1, y);
            board[x + 1, y + 1].SetGridPos(x + 1, y + 1);
            board[x, y + 1].SetGridPos(x, y + 1);
            board[x - 1, y + 1].SetGridPos(x - 1, y + 1);
        }
        else
        { // is odd
            BeadObject temp = board[x - 1, y - 1];
            board[x - 1, y - 1] = board[x - 1, y];
            board[x - 1, y] = board[x, y + 1];
            board[x, y + 1] = board[x + 1, y];
            board[x + 1, y] = board[x + 1, y - 1];
            board[x + 1, y - 1] = board[x, y - 1];
            board[x, y - 1] = temp;

            board[x - 1, y - 1].SetGridPos(x - 1, y - 1);
            board[x, y - 1].SetGridPos(x, y - 1);
            board[x + 1, y - 1].SetGridPos(x + 1, y - 1);
            board[x + 1, y].SetGridPos(x + 1, y);
            board[x, y + 1].SetGridPos(x, y + 1);
            board[x - 1, y].SetGridPos(x - 1, y);
        }
    }
}

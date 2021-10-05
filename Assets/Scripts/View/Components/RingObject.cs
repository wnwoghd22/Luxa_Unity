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
    private float angleOffset;
    public void SetAngleOffset(float f) => angleOffset = f;
    private bool isActivated;
    //public void SetActive(bool b) => isActivated = b;
    private Vector3 screenPos;
    private BeadObject[] beads;

    public void SetActive(bool b, BeadObject[,] board = null)
    {
        isActivated = b;
        if (b)
        {
            beads = new BeadObject[6];
            if (posX % 2 == 0)
            { // is even
                beads[0] = board[x - 1, y];
                beads[1] = board[x, y - 1];
                beads[2] = board[x + 1, y];
                beads[3] = board[x + 1, y + 1];
                beads[4] = board[x, y + 1];
                beads[5] = board[x - 1, y + 1];
            }
            else
            { // is odd
                beads[0] = board[x - 1, y - 1];
                beads[1] = board[x, y - 1];
                beads[2] = board[x + 1, y - 1];
                beads[3] = board[x + 1, y];
                beads[4] = board[x, y + 1];
                beads[5] = board[x - 1, y];
            }
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && isActivated) HandleRotate();
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
        //Debug.Log("rotate clockwise");
        beads = null;

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
        beads = null;

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

    public void SetAngleOffset(Vector3 v)
    {
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 vec3 = Input.mousePosition - screenPos;
        angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(vec3.y, vec3.x)) * Mathf.Rad2Deg;
    }
    public void HandleRotate()
    {
        //Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 vec3 = Input.mousePosition - screenPos;
        float angle = Mathf.Atan2(vec3.y, vec3.x);
        transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg + angleOffset);

        foreach (BeadObject bead in beads) bead.UpdatePosition(angle, transform.position);
    }
}

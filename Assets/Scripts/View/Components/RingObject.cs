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
    private float initialAngle;
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
            SetAlpha(0.7f);

            beads = new BeadObject[6];
            if (posX % 2 == 0)
            { // is even
                Debug.Log(posX + "even");
                beads[0] = board[x - 1, y];
                beads[1] = board[x, y - 1];
                beads[2] = board[x + 1, y];
                beads[3] = board[x + 1, y + 1];
                beads[4] = board[x, y + 1];
                beads[5] = board[x - 1, y + 1];
            }
            else
            { // is odd
                Debug.Log(posX + "odd");
                beads[0] = board[x - 1, y - 1];
                beads[1] = board[x, y - 1];
                beads[2] = board[x + 1, y - 1];
                beads[3] = board[x + 1, y];
                beads[4] = board[x, y + 1];
                beads[5] = board[x - 1, y];
            }
            SetAngleOffset();
            Debug.Log(beads.Length);
            foreach (BeadObject bead in beads) {
                Debug.Log("set angle");
                bead.SetAngleOffset(transform.position, angleOffset); 
            }

            HandleRotate();
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        angleOffset = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated) HandleRotate();
    }

    public override void SetGridPos(float offsetX, float offsetY, float unit, int x, int y, int z = 1)
    {
        SetPos(x, y);
        base.SetGridPos(offsetX, offsetY, unit, x, y, z);
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
        angleOffset = 0f;

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
        angleOffset = 0f;

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
    public void UndoRotate(BeadObject[,] board)
    {
        beads = null;
        angleOffset = 0f;

        if (posX % 2 == 0)
        { // is even
            board[x - 1, y].SetGridPos(x - 1, y);
            board[x, y - 1].SetGridPos(x, y - 1);
            board[x + 1, y].SetGridPos(x + 1, y);
            board[x + 1, y + 1].SetGridPos(x + 1, y + 1);
            board[x, y + 1].SetGridPos(x, y + 1);
            board[x - 1, y + 1].SetGridPos(x - 1, y + 1);
        }
        else
        { // is odd
            board[x - 1, y - 1].SetGridPos(x - 1, y - 1);
            board[x, y - 1].SetGridPos(x, y - 1);
            board[x + 1, y - 1].SetGridPos(x + 1, y - 1);
            board[x + 1, y].SetGridPos(x + 1, y);
            board[x, y + 1].SetGridPos(x, y + 1);
            board[x - 1, y].SetGridPos(x - 1, y);
        }
    }

    public void SetAngleOffset()
    {
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 vec3 = Input.mousePosition - screenPos;
        initialAngle = Mathf.Atan2(vec3.y, vec3.x);
        angleOffset = Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(vec3.y, vec3.x);
    }
    public void HandleRotate()
    {
        //Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 vec3 = Input.mousePosition - screenPos;
        float angle = Mathf.Atan2(vec3.y, vec3.x);
        transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg + angleOffset * Mathf.Rad2Deg);

        foreach (BeadObject bead in beads) bead.UpdatePosition(angle - initialAngle, transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour {

    [SerializeField] private GameObject beadPrefab;
    [SerializeField] private GameObject ringPrefab;

    private BeadObject[,] beadInstances;
    private RingObject[] ringInstances;

    private int _col, _row;

    private float unit;
    private float offsetX;
    private float offsetY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateTitleBoard()
    {
        DestroyBoard();

        _row = 3; _col = 4;
        beadInstances = new BeadObject[_row, _col];
        unit = (float)Camera.main.orthographicSize / (_col + 2);
        offsetX = -unit * (_col - 1) / 2;
        offsetY = unit * (_row - 1) * Mathf.Sqrt(3) / 4;
        int leftColor = Random.Range(0, 3), rightColor = Random.Range(0, 3);
        while (leftColor == rightColor) rightColor = Random.Range(0, 3); //assign different color

        for (int i = 0; i < _col - 1; ++i)
        {
            beadInstances[0, i] = Instantiate(beadPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<BeadObject>();
            beadInstances[0, i].SetGridPos(offsetX, offsetY, unit, 0, i);
            beadInstances[0, i].SetColor(
                (i != 2 && leftColor == 0) || (i != 0 && rightColor == 0) ? 1 : 0,
                (i != 2 && leftColor == 1) || (i != 0 && rightColor == 1) ? 1 : 0,
                (i != 2 && leftColor == 2) || (i != 0 && rightColor == 2) ? 1 : 0
            );
        }
        for (int i = 0; i < _col; ++i)
        {
            beadInstances[1, i] = Instantiate(beadPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<BeadObject>();
            beadInstances[1, i].SetGridPos(offsetX, offsetY, unit, 1, i);
            beadInstances[1, i].SetColor(
                (i != 3 && leftColor == 0) || (i != 0 && rightColor == 0) ? 1 : 0,
                (i != 3 && leftColor == 1) || (i != 0 && rightColor == 1) ? 1 : 0,
                (i != 3 && leftColor == 2) || (i != 0 && rightColor == 2) ? 1 : 0
            );
        }
        for (int i = 0; i < _col - 1; ++i)
        {
            beadInstances[2, i] = Instantiate(beadPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<BeadObject>();
            beadInstances[2, i].SetGridPos(offsetX, offsetY, unit, 2, i);
            beadInstances[2, i].SetColor(
                (i != 2 && leftColor == 0) || (i != 0 && rightColor == 0) ? 1 : 0,
                (i != 2 && leftColor == 1) || (i != 0 && rightColor == 1) ? 1 : 0,
                (i != 2 && leftColor == 2) || (i != 0 && rightColor == 2) ? 1 : 0
            );
        }

        ringInstances = new RingObject[2];
        for (int i = 0; i < 2; ++i)
        {
            ringInstances[i] = Instantiate(ringPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<RingObject>();
            ringInstances[i].SetGridPos(offsetX, offsetY, unit, 1, 1 + i, 1);
            ringInstances[i].SetColor(i == 0 ? leftColor : rightColor);
            ringInstances[i].SetIndex(i);
        }
    }

    public void CreateBoard(Board b) {
        DestroyBoard();

        Whole[,] _board = b.GetBoard();
        _row = _board.GetLength(0);
        _col = _board.GetLength(1);

        beadInstances = new BeadObject[_row, _col];

        unit = (float)Camera.main.orthographicSize / (_col + 2);
        //Debug.Log(Camera.main.orthographicSize);
        offsetX = - unit * (_col - 1) / 2;
        offsetY = unit * (_row - 1) * Mathf.Sqrt(3) / 4;
        //Debug.Log(_col + " " + _row + " " + offsetX + " " + offsetY + " " + unit);

        for (int i = 0; i < _row; ++i)
        {
            for (int j = 0; j < _col; ++j)
            {
                if (_board[i, j].Bead != null)
                {
                    beadInstances[i, j] = Instantiate(beadPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<BeadObject>();
                    beadInstances[i, j].SetGridPos(offsetX, offsetY, unit, i, j);
                    beadInstances[i, j].SetColor(_board[i, j].Bead.hasRed ? 1 : 0,
                                                 _board[i, j].Bead.hasGreen ? 1 : 0,
                                                 _board[i, j].Bead.hasBlue ? 1 : 0);
                }
            }
        }

        Ring[] rings = b.GetRings();
        ringInstances = new RingObject[rings.Length];
        for (int i = 0; i < rings.Length; ++i)
        {
            ringInstances[i] = Instantiate(ringPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<RingObject>();
            ringInstances[i].SetGridPos(offsetX, offsetY, unit, rings[i].posX, rings[i].posY, 1);
            ringInstances[i].SetColor(rings[i].Color);
            ringInstances[i].SetIndex(i);
        }
    }

    public void DestroyBoard()
    {
        if (beadInstances != null)
            foreach (BeadObject bead in beadInstances) if (bead != null) Destroy(bead.gameObject);
        if (ringInstances != null)
            foreach (RingObject ring in ringInstances) if (ring != null) Destroy(ring.gameObject);
    }

    public void UpdateBoard(int index, bool dir)
    {
        if (dir) ringInstances[index].RotateClockwise(beadInstances);
        else ringInstances[index].RotateCounterclockwise(beadInstances);
    }
    public void UndoRotate(int index) => ringInstances[index].UndoRotate(beadInstances);

    public void SetRingActivate(int i) => ringInstances[i].SetActive(true, beadInstances);
}

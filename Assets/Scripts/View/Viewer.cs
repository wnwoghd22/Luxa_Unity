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

    }

    public void CreateBoard(Board b) {
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

    public void UpdateBoard(int index, bool dir)
    {
        if (dir) ringInstances[index].RotateClockwise(beadInstances);
        else ringInstances[index].RotateCounterclockwise(beadInstances);
    }
    public void UndoRotate(int index) => ringInstances[index].UndoRotate(beadInstances);

    public void SetRingActivate(int i) => ringInstances[i].SetActive(true, beadInstances);
}

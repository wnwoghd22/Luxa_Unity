using System.Collections.Generic;

public class Board
{
	private Whole[,] board;
    public Whole[,] GetBoard() => board;

    private List<Ring> rings;
	public Ring[] GetRings() => rings.ToArray();

	private readonly int minimum;
	public int Minimum => minimum;

	public Board(int col, int row, int minimum) {
		board = new Whole[col, row];
		for (int i = 0; i < col; ++i)
			for (int j = 0; j < row; ++j)
				board[i, j] = new Whole();
		rings = new List<Ring>();
		this.minimum = minimum;
	}

    public void SetBeads(int x, int y, int r, int g, int b) => board[x, y].init(r, g, b);
    public void AddRing(int x, int y, int color, int i) => rings.Add(new Ring(x, y, color, i));

    public void Rotate(int ring, bool dir) => rings[ring].rotate(board, dir);

    private bool GetComplete() {
		bool result = true;
		foreach (Ring r in rings) if (!r.GetComplete(board)) return false;

		return result;
	}
	public bool isComplete { get => GetComplete(); }
}
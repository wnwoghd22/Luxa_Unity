using System.Collections.Generic;

public class Board
{
	private Whole[,] board;
	private List<Ring> rings;

	public Board(int col, int row) {
		board = new Whole[col, row];
		for (int i = 0; i < col; ++i)
			for (int j = 0; j < row; ++j)
				board[i, j] = new Whole();
		rings = new List<Ring>();
	}

    public void SetBeads(int x, int y, int r, int g, int b) => board[x, y].init(r, g, b);
    public void AddRing(int x, int y, int color) => rings.Add(new Ring(x, y, color));

    public void Rotate(int ring, int dir) => rings[ring].rotate(board, dir == 0);

    public bool GetComplete() {
		bool result = true;
		foreach (Ring r in rings) if (!r.GetComplete(board)) return false;

		return result;
	}
}
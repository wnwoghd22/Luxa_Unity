enum eColor {
	red,
	green,
	blue,
}

public class Ring {
	readonly int x, y;
	readonly eColor color;

	public Ring(int x, int y, int color) {
		this.x = x;
		this.y = y;
		this.color = (eColor)color;
	}

	/// <summary>
	/// 
	/// 
	/// </summary>
	/// <param name="board">2dimensional array of Whole class</param>
	/// <param name="dir">true = clockwise, false = counterclockwise</param>
	public void rotate(Whole[,] board, bool dir) {
		if (dir) RotateClock(board);
		else RotateCounter(board);

	}
	private void RotateClock(Whole[,] board) {
		if (x % 2 == 0) { // is even
			Bead temp = board[x - 1, y].Bead;
			board[x - 1, y].Bead = board[x, y - 1].Bead;
			board[x, y - 1].Bead = board[x + 1, y].Bead;
			board[x + 1, y].Bead = board[x + 1, y + 1].Bead;
			board[x + 1, y + 1].Bead = board[x, y + 1].Bead;
			board[x, y + 1].Bead = board[x - 1, y + 1].Bead;
			board[x - 1, y + 1].Bead = temp;
		}
		else { // is odd
			Bead temp = board[x - 1, y - 1].Bead;
			board[x - 1, y - 1].Bead = board[x, y - 1].Bead;
			board[x, y - 1].Bead = board[x + 1, y - 1].Bead;
			board[x + 1, y - 1].Bead = board[x + 1, y].Bead;
			board[x + 1, y].Bead = board[x, y + 1].Bead;
			board[x, y + 1].Bead = board[x - 1, y].Bead;
			board[x - 1, y].Bead = temp;
		}
	}
	private void RotateCounter(Whole[,] board) {
		if (x % 2 == 0) { // is even
			Bead temp = board[x - 1, y].Bead;
			board[x - 1, y].Bead = board[x - 1, y + 1].Bead;
			board[x - 1, y + 1].Bead = board[x, y + 1].Bead;
			board[x, y + 1].Bead = board[x + 1, y + 1].Bead;
			board[x + 1, y + 1].Bead = board[x + 1, y].Bead;
			board[x + 1, y].Bead = board[x, y - 1].Bead;
			board[x, y - 1].Bead = temp;
		}
		else { // is odd
			Bead temp = board[x - 1, y - 1].Bead;
			board[x - 1, y - 1].Bead = board[x - 1, y].Bead;
			board[x - 1, y].Bead = board[x, y + 1].Bead;
			board[x, y + 1].Bead = board[x + 1, y].Bead;
			board[x + 1, y].Bead = board[x + 1, y - 1].Bead;
			board[x + 1, y - 1].Bead = board[x, y - 1].Bead;
			board[x, y - 1].Bead = temp;
		}
	}

	private bool isMatch(Bead b) {
		bool result = false;
		switch (color) {
			case eColor.red:
				if (b.hasRed) result = true;
				break;
			case eColor.green:
				if (b.hasGreen) result = true;
				break;
			case eColor.blue:
				if (b.hasBlue) result = true;
				break;
		}

		return result;
	}
	public bool GetComplete(Whole[,] board) {
		bool result = false;
		bool centerMatch = board[x, y].Bead == null || isMatch(board[x, y].Bead);

		if (x % 2 == 0) { // is even
			result = (
				centerMatch &&
				isMatch(board[x - 1, y].Bead) &&
				isMatch(board[x - 1, y + 1].Bead) &&
				isMatch(board[x, y + 1].Bead) &&
				isMatch(board[x + 1, y + 1].Bead) &&
				isMatch(board[x + 1, y].Bead) &&
				isMatch(board[x, y - 1].Bead)
			);
		}
		else {
			result = (
				centerMatch &&
				isMatch(board[x - 1, y - 1].Bead) &&
				isMatch(board[x - 1, y].Bead) &&
				isMatch(board[x, y + 1].Bead) &&
				isMatch(board[x + 1, y].Bead) &&
				isMatch(board[x + 1, y - 1].Bead) &&
				isMatch(board[x, y - 1].Bead)
			);
		}
		return result;
	}
}
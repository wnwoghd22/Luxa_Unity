public class Whole {
	private Bead bead;
    public Bead Bead { get => bead; set => bead = value; }

	public Whole() {
		Bead = null;
	}
	public void init(int r, int g, int b) {
		Bead = new Bead(r, g, b);
	}
}

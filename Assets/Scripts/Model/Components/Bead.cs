public class Bead {
    public bool hasRed { get; private set; }
    public bool hasGreen { get; private set; }
    public bool hasBlue { get; private set; }

    public Bead(int r, int g, int b) {
		hasRed = (r == 1) ? true : false;
		hasGreen = (g == 1) ? true : false;
		hasBlue = (b == 1) ? true : false;
	}
}
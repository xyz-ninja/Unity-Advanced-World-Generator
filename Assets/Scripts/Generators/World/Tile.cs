
public class Tile {

	private HeightData _heightData;
	
	public float HeightValue { get; set; }
	public int Bitmask { get; set; }
	
	public int X, Y;

	public Tile Left { get; set; }
	public Tile Right { get; set; }
	public Tile Top { get; set; }
	public Tile Bottom { get; set; }

	public bool Solid;
	public bool FloodFilled; // тайлы уже обработанные алгоритмом заливки
	
	#region getters

	public HeightData HeightData => _heightData;

	#endregion
	
	public Tile() {
		
	}

	public void UpdateBitmask() {

		int count = 0;

		if (Top.HeightData.Index == _heightData.Index) { count += 1; }
		if (Right.HeightData.Index == _heightData.Index) { count += 2; }
		if (Bottom.HeightData.Index == _heightData.Index) { count += 4; }
		if (Left.HeightData.Index == _heightData.Index) { count += 8; }

		Bitmask = count;
	}

	public void SetHeightData(HeightData heightData) {
		_heightData = heightData;
	}
}

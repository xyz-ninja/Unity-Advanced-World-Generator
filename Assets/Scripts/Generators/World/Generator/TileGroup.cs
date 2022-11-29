using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILE_GROUP_TYPE {
	WATER, LAND
}

public class TileGroup {

	public TILE_GROUP_TYPE Type;

	private List<Tile> _tiles;

	#region getters

	public List<Tile> Tiles => _tiles;

	#endregion
	
	public TileGroup() {
		_tiles = new List<Tile>();
	}
	
	
}

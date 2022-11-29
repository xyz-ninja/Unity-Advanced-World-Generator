using UnityEngine;

public class TilesSearch : MonoBehaviour {
    
    [SerializeField] private Generator _generator;

    public void UpdateNeighbors() {
        for (int x = 0; x < _generator.Width; x++) {
            for (int y = 0; y < _generator.Height; y++) {
                
                Tile tile = _generator.Tiles[x, y];

                tile.Top = GetTop(tile);
                tile.Bottom = GetBottom(tile);
                tile.Left = GetLeft(tile);
                tile.Right = GetRight(tile);
            }
        }
    }
    
    public void UpdateBitmasks() {
        for (int x = 0; x < _generator.Width; x++) {
            for (int y = 0; y < _generator.Height; y++) {
                _generator.Tiles[x, y].UpdateBitmask();
            }
        }
    }

    // MathHelper.Mod() - сворачивает значения x и y на основании ширины и высоты карты.
    // что бы мы не могли выйти за пределы карты
    
    public Tile GetTop(Tile tile) {
        return _generator.Tiles[tile.X, MathHelper.Mod(tile.Y - 1, _generator.Height)];
    }

    public Tile GetBottom(Tile tile) {
        return _generator.Tiles[tile.X, MathHelper.Mod(tile.Y + 1, _generator.Height)];
    }

    public Tile GetLeft(Tile tile) {
        return _generator.Tiles[MathHelper.Mod(tile.X - 1, _generator.Width), tile.Y];
    }

    public Tile GetRight(Tile tile) {
        return _generator.Tiles[MathHelper.Mod(tile.X + 1, _generator.Width), tile.Y];
    }
}

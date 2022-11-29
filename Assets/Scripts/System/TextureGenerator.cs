using UnityEngine;

public static class TextureGenerator {

	public static Texture2D GetWorldGeneratorTexture(Generator generator, int width, int height, Tile[,] tiles) {
		
		var texture = new Texture2D(width, height);
		var pixels = new Color[width * height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				float heightValue = tiles[x, y].HeightValue;
				
				var heightData = generator.GetHeightDataByValue(heightValue);

				pixels[x + y * width] = heightData.Color;

				//затемняем цвет граничного тайла
				if (tiles[x, y].Bitmask != 15) {
					pixels[x + y * width] = Color.Lerp(pixels[x + y * width], Color.black, 0.4f);
				}

				//pixels[x + y * width] = Color.Lerp(Color.black, Color.white, heightValue);
			}
		}
		
		texture.SetPixels(pixels);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();

		return texture;
	}
}

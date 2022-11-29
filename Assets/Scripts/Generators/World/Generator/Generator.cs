using System;
using System.Collections.Generic;
using UnityEngine;
using AccidentalNoiseLibrary;
using NaughtyAttributes;

public class Generator : MonoBehaviour {

    [Header("Components")] 
    [SerializeField] private TilesSearch _tilesSearch;
    [SerializeField] private MeshRenderer _heightMapRenderer;

    [Header("Parametres")] 
    [SerializeField] private int _width = 256;
    [SerializeField] private int _height = 256;
    [SerializeField] private int _terrainOctaves = 6;
    [SerializeField] private double _terrainFrequency = 1.25;

    [Header("Height Datas")]
    [SerializeField] private List<HeightData> _heightDatas = new List<HeightData>();
    
    // модуль генератора шума
    private ImplicitFractal _heightMap;
    
    // карта высот
    private MapData _heightMapData;
    
    // конечные объекты
    private Tile[,] _tiles;

    #region getters

    public TilesSearch TilesSearch => _tilesSearch;

    public int Width => _width;
    public int Height => _height;

    public Tile[,] Tiles => _tiles;

    #endregion
    
    private void Start() {
        Generate();
    }

    [Button()]
    public void Generate() {
        
        Initialize();

        GetData(_heightMap, ref _heightMapData); // создаём карту высот
        LoadTiles();                             // создаём тайлы
        
        // рендерим текстуру
        _heightMapRenderer.materials[0].mainTexture = TextureGenerator.GetWorldGeneratorTexture(
            this, _width, _height, _tiles);
    }

    private void Initialize() {
        _heightMap = new ImplicitFractal(
            FractalType.Multi,
            BasisType.Simplex,
            InterpolationType.Quintic,
            _terrainOctaves,
            _terrainFrequency);
    }
    
    // извлекаем данные из модуля шума
    private void GetData(ImplicitModuleBase module, ref MapData mapData) {
        
        mapData = new MapData(_width, _height);
        
        // проходим по каждой точке x,y - получаем значение высоты
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                
                // пределы шума
                float x1 = 0, x2 = 1;
                float y1 = 0, y2 = 1;
                
                float dx = x2 - x1;
                float dy = y2 - y1;
                
                // сэмплируем шум с небольшими интервалами
                float s = x / (float) _width;
                float t = y / (float) _height;

                // вычисляем ебанные четырёхмерные координаты
                float nx = x1 + Mathf.Cos(s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
                float ny = y1 + Mathf.Cos(t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);
                float nz = x1 + Mathf.Sin(s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
                float nw = y1 + Mathf.Sin(t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);
                
                float heightValue = (float) _heightMap.Get(nx, ny, nz, nw);
                
                // отслеживаем максимальные и минимальные найденные значения
                if (heightValue > mapData.Max) { mapData.Max = heightValue; }
                if (heightValue < mapData.Min) { mapData.Min = heightValue; }

                mapData.Data[x, y] = heightValue;
            }
        }
    }
    
    // создаём массив тайлов из наших данных
    private void LoadTiles() {
        
        _tiles = new Tile[_width,_height];

        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                
                var tile = new Tile();
                
                tile.X = x;
                tile.Y = y;

                float heightValue = _heightMapData.Data[x, y];

                // нормализуем наше значение от 0 до 1
                heightValue = (heightValue - _heightMapData.Min) / (_heightMapData.Max - _heightMapData.Min);

                tile.HeightValue = heightValue;
                
                tile.SetHeightData(GetHeightDataByValue(heightValue));

                _tiles[x, y] = tile;
            }
        }
        
        _tilesSearch.UpdateNeighbors();
        _tilesSearch.UpdateBitmasks();
    }

    public HeightData GetHeightDataByValue(float value) {
        foreach (var heightData in _heightDatas) {
            if (value <= heightData.HeightValue) {
                return heightData;
            }
        }

        Debug.Log("HeightData not found!");
        return _heightDatas[0];
    }
}
 
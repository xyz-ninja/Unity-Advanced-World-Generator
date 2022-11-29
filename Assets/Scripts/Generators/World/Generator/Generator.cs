using System;
using System.Collections.Generic;
using UnityEngine;
using AccidentalNoiseLibrary;

public class Generator : MonoBehaviour {
    
    [Header("Components")] 
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
    private MapData _heightData;
    
    // конечные объекты
    private Tile[,] _tiles;

    private void Start() {
        
        Initialize();
        
        GetData(_heightMap, ref _heightData); // создаём карту высот
        LoadTiles(); // создаём тайлы
        
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
                
                // сэмплируем шум с небольшими интервалами
                float x1 = x / (float) _width;
                float y1 = y / (float) _height;

                float value = (float) _heightMap.Get(x1, y1);
                
                // отслеживаем максимальные и минимальные найденные значения
                if (value > mapData.Max) { mapData.Max = value; }
                if (value < mapData.Min) { mapData.Min = value; }

                mapData.Data[x, y] = value;
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

                float value = _heightData.Data[x, y];

                // нормализуем наше значение от 0 до 1
                value = (value - _heightData.Min) / (_heightData.Max - _heightData.Min);

                tile.HeightValue = value;

                _tiles[x, y] = tile;
            }
        }
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
 
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap};
    public DrawMode drawMode;


    [Range(50, 300)]
    public int mapWidth;
    [Range(50, 300)]
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    [Range(1, 10)]
    public float lacunarity;

    public bool autoUpdate;

    public TerrainType[] regions;

    public int seed;
    public Vector2 offset;
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[mapHeight * mapWidth];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapWidth + x] = regions[i].colour;
                        break;
                    }

                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap) { display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap)); }
        else if (drawMode == DrawMode.ColourMap) { display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight)); }
/*        display.DrawTexture(noiseMap); */ 
    }

    void OnValidate()
    {
        if (mapWidth < 1)   mapWidth = 1;
        if (mapHeight < 1)  mapHeight = 1;
        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 1)    octaves = 1;
    }

}
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
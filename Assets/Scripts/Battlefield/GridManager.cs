using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int  width  = 0;
    [SerializeField] private int  height = 0;
    [SerializeField] private Tile tile;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                var spawnedTIle = Instantiate(tile, new Vector3(x, y), Quaternion.identity);
                spawnedTIle.name = $"Tile {x}, {y}";
            }    
        }
        
    }
}
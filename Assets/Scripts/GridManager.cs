using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int gridWidth;
    [SerializeField] int gridHeight;
    [SerializeField] GameObject gridCellVisual;
    [SerializeField] bool ShowCellVisual;
    [SerializeField] Transform cellVisualParent;
    Vector2 startPoint;
    private void Awake()
    {
        float startPointX = -(float)gridWidth / 2 + 0.5f;
        float startPointY = (float)gridHeight / 2 - 0.5f;

        startPoint = new Vector2(startPointX, startPointY);
    }
    void Start()
    {
        InitializeGrid();
    }

    void Update()
    {

    }
    void InitializeGrid()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (ShowCellVisual)
                {
                    Vector2 spawnPoint = startPoint + new Vector2Int(i, -j);
                    GameObject cellVisualGO = Instantiate(gridCellVisual, spawnPoint, Quaternion.identity);
                    cellVisualGO.transform.SetParent(cellVisualParent);

                }
            }
        }
    }
}

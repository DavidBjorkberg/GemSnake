using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    [SerializeField] Wall mWallPrefab;
    GridManager mGridManager;
    private void Awake()
    {
        mGridManager = GetComponent<GridManager>();
    }
    private void Start()
    {
        SpawnBoundary();
    }
    public void SpawnBoundary()
    {
        int gridWidth = mGridManager.GetGridWidth();
        int gridHeight = mGridManager.GetGridHeight();
        for (int i = 0; i < gridWidth; i++) //Top 
        {
            SpawnWall(mGridManager.GetCell(i));
        }
        for (int i = 0; i < gridHeight; i++) //Left
        {
            int cellIndex = gridWidth * i;
            SpawnWall(mGridManager.GetCell(cellIndex));
        }
        for (int i = 0; i < gridHeight; i++) // right
        {
            int cellIndex = (gridWidth * i) + gridWidth - 1;
            SpawnWall(mGridManager.GetCell(cellIndex));
        }
        for (int i = 0; i < gridWidth; i++) //Bottom
        {
            int cellIndex = i + (gridWidth * (gridHeight - 1));
            SpawnWall(mGridManager.GetCell(cellIndex));
        }

    }
    void SpawnWall(GridManager.CellInfo cell)
    {
        Wall spawnedWall = Instantiate(mWallPrefab, cell.pos, Quaternion.identity);
        spawnedWall.AddToGrid(cell.index);

    }
}

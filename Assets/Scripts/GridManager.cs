using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public struct CellInfo
    {
        public Vector3 pos;
        public int index;
        public Entity entity;
        public CellInfo(Vector3 pos, int index)
        {
            this.pos = pos;
            this.index = index;
            this.entity = null;
        }
    };
    CellInfo[] grid;
    [SerializeField] int gridWidth;
    [SerializeField] int gridHeight;
    [SerializeField] GameObject gridCellVisual;
    [SerializeField] bool ShowCellVisual;
    [SerializeField] Transform cellVisualParent;
    Vector2 startPoint;
    private void Awake()
    {
        grid = new CellInfo[gridWidth * gridHeight];
        float startPointX = -(float)gridWidth / 2 + 0.5f;
        float startPointY = (float)gridHeight / 2 - 0.5f;

        startPoint = new Vector2(startPointX, startPointY);
        InitializeGrid();
    }
    void InitializeGrid()
    {
        int cellIndex = 0;
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                Vector2 spawnPoint = startPoint + new Vector2Int(j, -i);
                grid[cellIndex] = new CellInfo(spawnPoint, cellIndex);
                cellIndex++;
                if (ShowCellVisual)
                {
                    GameObject cellVisualGO = Instantiate(gridCellVisual, spawnPoint, Quaternion.identity);
                    cellVisualGO.transform.SetParent(cellVisualParent);
                }
            }
        }
    }
    public CellInfo GetCellBehind(int cellIndex, GameplayStatics.Direction forwardDir)
    {
        int returnCellIndex = cellIndex;

        switch (forwardDir)
        {
            case GameplayStatics.Direction.Up:
                returnCellIndex += gridWidth;
                break;
            case GameplayStatics.Direction.Right:
                returnCellIndex--;
                break;
            case GameplayStatics.Direction.Down:
                returnCellIndex -= gridWidth;
                break;
            case GameplayStatics.Direction.Left:
                returnCellIndex++;
                break;
            default:
                break;
        }
        return grid[returnCellIndex];
    }
    public CellInfo GetCellAhead(int cellIndex, GameplayStatics.Direction forwardDir)
    {
        int returnCellIndex = cellIndex;

        switch (forwardDir)
        {
            case GameplayStatics.Direction.Up:
                returnCellIndex -= gridWidth;
                break;
            case GameplayStatics.Direction.Right:
                returnCellIndex++;
                break;
            case GameplayStatics.Direction.Down:
                returnCellIndex += gridWidth;
                break;
            case GameplayStatics.Direction.Left:
                returnCellIndex--;
                break;
            default:
                break;
        }
        return grid[returnCellIndex];
    }
    public ref CellInfo GetCell(int index)
    {
        return ref grid[index];
    }
    public void AddEntityToCell(Entity entity, int index)
    {
        grid[index].entity = entity;
    }
    public void MoveEntity(int startIndex, int targetIndex)
    {
        CellInfo startcell = GetCell(startIndex);
        CellInfo targetCell = GetCell(targetIndex);

        if (startcell.entity == null)
        {
            Debug.LogError("Tried to move entity from cell that did not have an entity");
            return;
        }
        grid[targetIndex].entity = grid[startIndex].entity;
        grid[targetIndex].entity.SetCurCellIndex(grid[targetIndex].index);
        grid[startIndex].entity = null;
    }
    public void SteppedOnCell(CellInfo cell)
    {
        if (cell.entity != null)
        {
            cell.entity.SteppedOn();
        }
    }
    /// <summary>
    /// Function assumes startIndex and targetIndex are next to eachother on the grid
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="targetIndex"></param>
    /// <returns></returns>
    public GameplayStatics.Direction GetDirectionToCell(int startIndex, int targetIndex)
    {
        GameplayStatics.Direction returnDir = GameplayStatics.Direction.Invalid;

        if (targetIndex == startIndex + 1)
        {
            returnDir = GameplayStatics.Direction.Right;
        }
        else if (targetIndex == startIndex - 1)
        {
            returnDir = GameplayStatics.Direction.Left;
        }
        else if (targetIndex == startIndex - gridWidth)
        {
            returnDir = GameplayStatics.Direction.Up;
        }
        else if (targetIndex == startIndex + gridWidth)
        {
            returnDir = GameplayStatics.Direction.Down;
        }
        return returnDir;
    }
}

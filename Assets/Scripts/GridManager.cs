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
    [SerializeField] int mGridWidth;
    [SerializeField] int mGridHeight;
    [SerializeField] GameObject mGridCellVisual;
    [SerializeField] bool mShowCellVisual;
    [SerializeField] Transform mCellVisualParent;
    CellInfo[] mGrid;
    List<GameObject> mVisualGrid = new List<GameObject>();
    Vector2 mStartPoint;
    private void Awake()
    {
        mGrid = new CellInfo[mGridWidth * mGridHeight];
        float startPointX = -(float)mGridWidth / 2 + 0.5f;
        float startPointY = (float)mGridHeight / 2 - 0.5f;

        mStartPoint = new Vector2(startPointX, startPointY);
        InitializeGrid();
    }
    void InitializeGrid()
    {
        int cellIndex = 0;
        for (int i = 0; i < mGridHeight; i++)
        {
            for (int j = 0; j < mGridWidth; j++)
            {
                Vector2 spawnPoint = mStartPoint + new Vector2Int(j, -i);
                mGrid[cellIndex] = new CellInfo(spawnPoint, cellIndex);
                cellIndex++;
                if (mShowCellVisual)
                {
                    GameObject cellVisualGO = Instantiate(mGridCellVisual, spawnPoint, Quaternion.identity);
                    cellVisualGO.transform.SetParent(mCellVisualParent);
                    mVisualGrid.Add(cellVisualGO);
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
                returnCellIndex += mGridWidth;
                break;
            case GameplayStatics.Direction.Right:
                returnCellIndex--;
                break;
            case GameplayStatics.Direction.Down:
                returnCellIndex -= mGridWidth;
                break;
            case GameplayStatics.Direction.Left:
                returnCellIndex++;
                break;
            default:
                break;
        }
        return mGrid[returnCellIndex];
    }
    public CellInfo GetCellAhead(int cellIndex, GameplayStatics.Direction forwardDir)
    {
        int returnCellIndex = cellIndex;

        switch (forwardDir)
        {
            case GameplayStatics.Direction.Up:
                returnCellIndex -= mGridWidth;
                break;
            case GameplayStatics.Direction.Right:
                returnCellIndex++;
                break;
            case GameplayStatics.Direction.Down:
                returnCellIndex += mGridWidth;
                break;
            case GameplayStatics.Direction.Left:
                returnCellIndex--;
                break;
            default:
                break;
        }
        return mGrid[returnCellIndex];
    }
    public ref CellInfo GetCellAtPos(Vector3 pos)
    {
        for (int i = 0; i < mGrid.Length; i++)
        {
            if (mGrid[i].pos.x - 0.5f <= pos.x &&
                mGrid[i].pos.x + 0.5f >= pos.x &&
                mGrid[i].pos.y - 0.5f <= pos.y &&
                mGrid[i].pos.y + 0.5f >= pos.y)
            {
                return ref mGrid[i];
            }
        }
        Debug.LogError("Couldn't find cell at Pos");
        return ref mGrid[0];
    }
    public ref CellInfo GetCell(int index)
    {
        return ref mGrid[index];
    }
    public void AddEntityToCell(Entity entity, int index)
    {
        mGrid[index].entity = entity;
    }
    public void MoveEntity(int startIndex, int targetIndex)
    {
        if (startIndex == targetIndex)
        {
            return;
        }
        ref CellInfo startcell = ref GetCell(startIndex);
        ref CellInfo targetCell = ref GetCell(targetIndex);

        if (startcell.entity == null)
        {
            Debug.LogError("Tried to move entity from cell that did not have an entity");
            return;
        }
        targetCell.entity = startcell.entity;
        targetCell.entity.SetCurCellIndex(targetCell.index);
        startcell.entity = null;
    }
    public void SteppedOnCell(CellInfo cell)
    {
        if (cell.entity)
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
        else if (targetIndex == startIndex - mGridWidth)
        {
            returnDir = GameplayStatics.Direction.Up;
        }
        else if (targetIndex == startIndex + mGridWidth)
        {
            returnDir = GameplayStatics.Direction.Down;
        }
        return returnDir;
    }
    public CellInfo GetRandomFreeCell()
    {
        CellInfo randomCell;
        do
        {
            int randomCellIndex = Random.Range(0, mGrid.Length);
            randomCell = mGrid[randomCellIndex];

        } while (randomCell.entity);

        return randomCell;
    }
    public int GetGridWidth()
    {
        return mGridWidth;
    }
    public int GetGridHeight()
    {
        return mGridHeight;
    }
    public List<GameObject> GetVisualGrid()
    {
        return mVisualGrid;
    }

}

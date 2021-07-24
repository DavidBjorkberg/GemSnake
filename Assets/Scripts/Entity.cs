using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    int mCurCellIndex;
    bool mPartOfCombo;
    public abstract void SteppedOn();
    public int GetCurCellIndex()
    {
        return mCurCellIndex;
    }
    public void SetCurCellIndex(int cellIndex)
    {
        mCurCellIndex = cellIndex;
    }
    public void AddToGrid(int cellIndex)
    {
        GridManager gridManager = GameManager.Instance.GetGridManager();
        SetCurCellIndex(cellIndex);
        gridManager.AddEntityToCell(this, GetCurCellIndex());
    }
    public virtual GemManager.GemType GetGemType()
    {
        return GemManager.GemType._MAX;
    }
    public bool IsPartOfCombo()
    {
        return mPartOfCombo;
    }
    public void SetIsPartOfCombo(bool isPartOfCombo)
    {
        mPartOfCombo = isPartOfCombo;
    }
}

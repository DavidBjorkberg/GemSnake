using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    int curCellIndex;
    public abstract void SteppedOn();
    public int GetCurCellIndex()
    {
        return curCellIndex;
    }
    public void SetCurCellIndex(int cellIndex)
    {
        curCellIndex = cellIndex;
    }
}

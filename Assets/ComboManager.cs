using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    const int MIN_COMBO_AMOUNT = 3;
    [SerializeField] ParticleSystem comboParticleSystem;
    GridManager mGridManager;
    SnakeBodyManager mBodyManager;
    List<int> mNeighboursOffsets = new List<int>();
    private void Awake()
    {
        mGridManager = GetComponent<GridManager>();
    }
    private void Start()
    {
        mBodyManager = GameManager.Instance.GetPlayer().GetComponent<SnakeBodyManager>();
        InitializeNeighbourOffsets();
    }
    public void ComboCheck()
    {
        List<BodyPart> allBodyParts = mBodyManager.GetAllBodyParts();
        allBodyParts.RemoveAt(0); //Remove head

        foreach (BodyPart body in allBodyParts)
        {
            CheckBodyForCombo(body);
        }
    }
    void CheckBodyForCombo(BodyPart body)
    {
        foreach (int neighbourOffset in mNeighboursOffsets)
        {
            int bodyCellIndex = body.GetCurCellIndex();
            TryStartComboChain(bodyCellIndex, bodyCellIndex + neighbourOffset);
        }
    }
    void TryStartComboChain(int startCellIndex, int secondCellIndex)
    {
        Entity startEntity = mGridManager.GetCell(startCellIndex).entity;
        Entity secondEntity = mGridManager.GetCell(secondCellIndex).entity;
        if (!startEntity || !secondEntity || startEntity.GetGemType() != secondEntity.GetGemType() || startEntity.IsPartOfCombo())
        {
            return;
        }

        List<BodyPart> comboList = new List<BodyPart>();
        comboList.Add(startEntity as BodyPart);
        TryAddEntityToCombo(secondCellIndex, startCellIndex, ref comboList);
    }
    void TryAddEntityToCombo(int cellIndex, int prevCellIndex, ref List<BodyPart> comboList)
    {
        if (!DoesComboContinue(cellIndex, prevCellIndex))
        {
            ComboEnded(comboList);
            return;
        }

        BodyPart newComboPart = mGridManager.GetCell(cellIndex).entity as BodyPart;
        comboList.Add(newComboPart);


        int offsetToNext = cellIndex - prevCellIndex;
        TryAddEntityToCombo(cellIndex + offsetToNext, cellIndex, ref comboList);
    }
    public void ComboEnded(List<BodyPart> comboList)
    {
        if (comboList.Count >= MIN_COMBO_AMOUNT)
        {
            GameManager.Instance.GetStepManager().PerformedCombo(comboList);
            foreach (BodyPart bodyPart in comboList)
            {
                bodyPart.SetIsPartOfCombo(true);
            }
            mBodyManager.MoveBodyAfterCombo();

            foreach (BodyPart bodyPart in comboList)
            {
                // DestroyBodyPart(bodyPart);
               // bodyPart.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
    bool DoesComboContinue(int cellIndex, int prevCellIndex)
    {
        int offsetToNext = cellIndex - prevCellIndex;
        int nextCellIndex = cellIndex + offsetToNext;
        bool nextCellValid = IsCellValidForCombo(cellIndex, nextCellIndex);
        if (!nextCellValid)
        {
            return false;
        }

        Entity entityToCheck = mGridManager.GetCell(cellIndex).entity;
        Entity prevBody = mGridManager.GetCell(prevCellIndex).entity;
        bool bothEntitiesValid = entityToCheck && prevBody && !entityToCheck.IsPartOfCombo();
        if (!bothEntitiesValid)
        {
            return false;
        }

        bool gemsMatch = entityToCheck.GetGemType() == prevBody.GetGemType();

        return gemsMatch;
    }
    bool IsCellValidForCombo(int cellIndex, int nextCellIndex)
    {
        bool isInsideGridBounds = nextCellIndex >= 0 &&
            nextCellIndex < (mGridManager.GetGridHeight() * mGridManager.GetGridWidth());
        bool movedPastEdge;
        if (Mathf.Abs(cellIndex - nextCellIndex) == 1) //Pure left / right
        {
            movedPastEdge = Mathf.FloorToInt((float)nextCellIndex / mGridManager.GetGridWidth()) != Mathf.FloorToInt(cellIndex / mGridManager.GetGridWidth());
        }
        else
        {
            int curRow = Mathf.FloorToInt(cellIndex / mGridManager.GetGridWidth());
            int nextRow = Mathf.FloorToInt((float)nextCellIndex / mGridManager.GetGridWidth());
            movedPastEdge = Mathf.Abs(curRow - nextRow) > 1; //The next cell is two steps up, which means the edge was crossed
        }

        return isInsideGridBounds &&
            !movedPastEdge;
    }
    public void DestroyBodyPart(BodyPart bodyPart)
    {
        Instantiate(comboParticleSystem, bodyPart.transform.position, Quaternion.identity);
        Destroy(bodyPart.gameObject);
    }
    void InitializeNeighbourOffsets()
    {
        int gridWidth = mGridManager.GetGridWidth();
        mNeighboursOffsets.Add(-gridWidth - 1);
        mNeighboursOffsets.Add(-gridWidth);
        mNeighboursOffsets.Add(-gridWidth + 1);
        mNeighboursOffsets.Add(1);
        mNeighboursOffsets.Add(gridWidth + 1);
        mNeighboursOffsets.Add(gridWidth);
        mNeighboursOffsets.Add(gridWidth - 1);
        mNeighboursOffsets.Add(-1);
    }
}

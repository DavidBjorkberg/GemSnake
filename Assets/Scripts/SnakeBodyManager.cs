using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyManager : MonoBehaviour
{
    [SerializeField] int mNrOfBodyPartsAtSpawn;
    [SerializeField] int mStartCellIndex;
    [SerializeField] BodyPart mHead;
    [SerializeField] BodyPart mBodyPartPrefab;
    [SerializeField] GridManager mGridManager;

    void Start()
    {
        transform.position = mGridManager.GetCell(mStartCellIndex).pos;
        mHead.AddToGrid(mStartCellIndex);
        
        for (int i = 0; i < mNrOfBodyPartsAtSpawn; i++)
        {
            AddBodyPart(GameManager.Instance.GetGemManager().GetGemTypeToSpawn());
        }
    }

    public void MoveSnake(GridManager.CellInfo targetCell)
    {
        MoveHead(targetCell);
        MoveBody();
    }
    void MoveHead(GridManager.CellInfo targetCell)
    {
        mHead.transform.position = targetCell.pos;
        mGridManager.MoveEntity(mHead.GetCurCellIndex(), targetCell.index);
    }
    void MoveBody()
    {
        BodyPart curBodyPart = mHead;
        while (curBodyPart.GetPreviousBodyPart() != null)
        {
            curBodyPart = curBodyPart.GetPreviousBodyPart();
            BodyPart nextBodyPart = curBodyPart.GetNextBodyPart();
            GridManager.CellInfo targetCell = mGridManager.GetCellBehind(nextBodyPart.GetCurCellIndex(), nextBodyPart.GetDirection());
            GameplayStatics.Direction dirToTarget = mGridManager.GetDirectionToCell(curBodyPart.GetCurCellIndex(), targetCell.index);
            if (dirToTarget != GameplayStatics.Direction.Invalid)
            {
                curBodyPart.SetDirection(dirToTarget);
            }
            curBodyPart.transform.position = targetCell.pos;
            mGridManager.MoveEntity(curBodyPart.GetCurCellIndex(), targetCell.index);
        }
    }
    public void AddBodyPart(GemManager.GemType gemType)
    {
        BodyPart tail = GetTail();
        BodyPart newBody = SpawnBody();
        LinkBodyPartToBody(newBody, tail);
        InitializeBodyAsGem(newBody, gemType);
        InitializeBodyAsEntity(newBody, tail);
    }
    BodyPart SpawnBody()
    {
        BodyPart tail = GetTail();
        Vector3 spawnPos = mGridManager.GetCell(tail.GetCurCellIndex()).pos - tail.transform.up;
        BodyPart newBody = Instantiate(mBodyPartPrefab, spawnPos, Quaternion.identity);
        return newBody;
    }
    void LinkBodyPartToBody(BodyPart newBody, BodyPart tail)
    {
        newBody.SetNextBodyPart(tail);
        newBody.SetDirection(tail.GetDirection());
        tail.SetPreviousBodyPart(newBody);
    }
    void InitializeBodyAsGem(BodyPart newBody, GemManager.GemType gemType)
    {
        GemManager gemManager = GameManager.Instance.GetGemManager();
        newBody.SetGemType(gemType);
        Animator animator = newBody.GetComponent<Animator>();
        animator.runtimeAnimatorController = gemManager.GetGemAnimController(gemType);
    }
    void InitializeBodyAsEntity(BodyPart newBody, BodyPart tail)
    {
        int cellIndex = mGridManager.GetCellBehind(tail.GetCurCellIndex(), tail.GetDirection()).index;
        newBody.AddToGrid(cellIndex);
    }

    BodyPart GetTail()
    {
        BodyPart returnBodyPart = mHead;

        while (returnBodyPart.GetPreviousBodyPart() != null)
        {
            returnBodyPart = returnBodyPart.GetPreviousBodyPart();
        }
        return returnBodyPart;
    }
    public BodyPart GetHead()
    {
        return mHead;
    }
}

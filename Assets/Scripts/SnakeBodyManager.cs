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
        mHead.SetGemType(GemManager.GemType._MAX);

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

            curBodyPart.transform.position = targetCell.pos;
            mGridManager.MoveEntity(curBodyPart.GetCurCellIndex(), targetCell.index);
            if (dirToTarget != GameplayStatics.Direction.Invalid)
            {
                curBodyPart.SetDirection(dirToTarget);
            }
            else
            {
                Debug.LogError("DirToTarget was Invalid ");
            }
        }
    }
    public void MoveBodyAfterCombo()
    {
        BodyPart curBodyPart = mHead;
        int nrOfStepsToMove = 1;
        while (curBodyPart.GetPreviousBodyPart() != null)
        {
            curBodyPart = curBodyPart.GetPreviousBodyPart();
            if (curBodyPart.IsPartOfCombo())
            {
                List<BodyPart> bodyPartsToNextCombo = GetAllBodiesUpToNextCombo(curBodyPart);
                if (bodyPartsToNextCombo.Count == 0)
                {
                    nrOfStepsToMove++;
                    continue;
                }
                foreach (BodyPart bodyPart in bodyPartsToNextCombo)
                {
                    StartCoroutine(bodyPart.MoveToNextCell(nrOfStepsToMove));
                }
                nrOfStepsToMove = 1;
                curBodyPart = bodyPartsToNextCombo[bodyPartsToNextCombo.Count - 1];
            }

        }
    }
    List<BodyPart> GetAllBodiesUpToNextCombo(BodyPart startBodyPart)
    {
        BodyPart curBodyPart = startBodyPart;
        List<BodyPart> returnList = new List<BodyPart>();
        while (curBodyPart.GetPreviousBodyPart() && !curBodyPart.GetPreviousBodyPart().IsPartOfCombo())
        {
            curBodyPart = curBodyPart.GetPreviousBodyPart();
            returnList.Add(curBodyPart);
        }

        return returnList;
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
    //Returns all body parts of the snake, head at 0, tail at last
    public List<BodyPart> GetAllBodyParts()
    {
        List<BodyPart> allBodyParts = new List<BodyPart>();
        allBodyParts.Add(mHead);

        while (allBodyParts[allBodyParts.Count - 1].GetPreviousBodyPart() != null)
        {
            allBodyParts.Add(allBodyParts[allBodyParts.Count - 1].GetPreviousBodyPart());
        }
        return allBodyParts;
    }
    public void RemoveBodyPart(BodyPart bodyPart)
    {
        BodyPart prevBodyPart = bodyPart.GetPreviousBodyPart();
        BodyPart nextBodyPart = bodyPart.GetNextBodyPart();

        if (prevBodyPart)
        {
            nextBodyPart.SetPreviousBodyPart(prevBodyPart);
            prevBodyPart.SetNextBodyPart(nextBodyPart);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyManager : MonoBehaviour
{
    [SerializeField] int nrOfBodyPartsAtSpawn;
    [SerializeField] int startCellIndex;
    [SerializeField] BodyPart head;
    [SerializeField] BodyPart bodyPartPrefab;
    [SerializeField] GridManager gridManager;

    void Start()
    {
        transform.position = gridManager.GetCell(startCellIndex).pos;

        gridManager.AddEntityToCell(head, startCellIndex);
        for (int i = 0; i < nrOfBodyPartsAtSpawn; i++)
        {
            AddBodyPart();
        }
    }

    public void MoveSnake( GridManager.CellInfo targetCell)
    {
        MoveHead(targetCell);
        MoveBody();
    }
    void MoveHead( GridManager.CellInfo targetCell)
    {
        head.transform.position = targetCell.pos;
        gridManager.MoveEntity(head.GetCurCellIndex(), targetCell.index);
    }
    void MoveBody()
    {
        BodyPart curBodyPart = head;
        while (curBodyPart.GetPreviousBodyPart() != null)
        {
            curBodyPart = curBodyPart.GetPreviousBodyPart();
            BodyPart nextBodyPart = curBodyPart.GetNextBodyPart();
            GridManager.CellInfo targetCell = gridManager.GetCellBehind(nextBodyPart.GetCurCellIndex(), nextBodyPart.GetDirection());
            GameplayStatics.Direction dirToTarget = gridManager.GetDirectionToCell(curBodyPart.GetCurCellIndex(), targetCell.index);
            if (dirToTarget != GameplayStatics.Direction.Invalid)
            {
                curBodyPart.SetDirection(dirToTarget);
            }
            curBodyPart.transform.position = targetCell.pos;
            gridManager.MoveEntity(curBodyPart.GetCurCellIndex(), targetCell.index);
        }
    }
    void AddBodyPart()
    {
        BodyPart tail = GetTail();
        Vector3 spawnPos = gridManager.GetCell(tail.GetCurCellIndex()).pos - tail.transform.up;
        BodyPart newBody = Instantiate(bodyPartPrefab, spawnPos, Quaternion.identity);

        newBody.SetNextBodyPart(tail);
        newBody.SetCurCellIndex(gridManager.GetCellBehind(tail.GetCurCellIndex(), tail.GetDirection()).index);
        newBody.SetDirection(tail.GetDirection());
        tail.SetPreviousBodyPart(newBody);

        gridManager.AddEntityToCell(newBody, newBody.GetCurCellIndex());
    }
    BodyPart GetTail()
    {
        BodyPart returnBodyPart = head;

        while (returnBodyPart.GetPreviousBodyPart() != null)
        {
            returnBodyPart = returnBodyPart.GetPreviousBodyPart();
        }
        return returnBodyPart;
    }
    public BodyPart GetHead()
    {
        return head;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : Entity
{
    BodyPart mPrevPart;
    BodyPart mNextPart;
    GameplayStatics.Direction mDirection;
    GemManager.GemType mGemType;
    public void SetNextBodyPart(BodyPart newNext)
    {
        mNextPart = newNext;
    }
    public BodyPart GetNextBodyPart()
    {
        return mNextPart;
    }
    public void SetPreviousBodyPart(BodyPart newPrev)
    {
        mPrevPart = newPrev;
    }
    public BodyPart GetPreviousBodyPart()
    {
        return mPrevPart;
    }
    public IEnumerator MoveToNextCell(int nrOfStepsToMove)
    {
        GridManager gridManager = GameManager.Instance.GetGridManager();
        BodyPart targetBodyPart = mNextPart;

        for (int i = 0; i < nrOfStepsToMove; i++)
        {
            GridManager.CellInfo startCell = gridManager.GetCell(targetBodyPart.GetPreviousBodyPart().GetCurCellIndex());
            GridManager.CellInfo targetCell = gridManager.GetCell(targetBodyPart.GetCurCellIndex());
            Vector3 startPos = startCell.pos;
            Vector3 targetPos = targetCell.pos;
            float lerpValue = 0;

            while (lerpValue < 1)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, lerpValue);
                lerpValue += Time.deltaTime / StepManager.STEP_RATE * nrOfStepsToMove;
                yield return new WaitForEndOfFrame();
            }
            targetBodyPart = targetBodyPart.GetNextBodyPart();
        }

    }

    public override void SteppedOn()
    {
        GameManager.Instance.EndGame();
    }
    public void SetDirection(GameplayStatics.Direction newDir)
    {
        mDirection = newDir;
    }
    public GameplayStatics.Direction GetDirection()
    {
        return mDirection;
    }
    public void SetGemType(GemManager.GemType gemType)
    {
        mGemType = gemType;
    }
    public override GemManager.GemType GetGemType()
    {
        return mGemType;
    }
}

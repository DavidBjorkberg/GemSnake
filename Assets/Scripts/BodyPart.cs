using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : Entity
{
    BodyPart prevPart;
    BodyPart nextPart;
    GameplayStatics.Direction direction;
    public void SetNextBodyPart(BodyPart newNext)
    {
        nextPart = newNext;
    }
    public BodyPart GetNextBodyPart()
    {
        return nextPart;
    }
    public void SetPreviousBodyPart(BodyPart newPrev)
    {
        prevPart = newPrev;
    }
    public BodyPart GetPreviousBodyPart()
    {
        return prevPart;
    }

    public override void SteppedOn()
    {
        GameManager.Instance.EndGame();
    }
    public void SetDirection(GameplayStatics.Direction newDir)
    {
        direction = newDir;
    }
    public GameplayStatics.Direction GetDirection()
    {
        return direction;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Entity
{
    public override void SteppedOn()
    {
        GameManager.Instance.EndGame();
    }
}

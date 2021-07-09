using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Entity
{
    public override void SteppedOn()
    {
        SnakeBodyManager snakeBodyManager = GameManager.Instance.GetPlayer().GetComponent<SnakeBodyManager>();

        snakeBodyManager.AddBodyPart();
        Destroy(gameObject);
    }
}

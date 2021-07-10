using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Entity
{
    GemManager.GemType mGemType;
    private void Start()
    {
    }
    public override void SteppedOn()
    {
        SnakeBodyManager snakeBodyManager = GameManager.Instance.GetPlayer().GetComponent<SnakeBodyManager>();

        snakeBodyManager.AddBodyPart(mGemType);
        Destroy(gameObject);
    }
    public void InitializeGem(GemManager.GemType gemType)
    {
        Animator animator = GetComponent<Animator>();
        GemManager gemManager = GameManager.Instance.GetGemManager();
        this.mGemType = gemType;
        animator.runtimeAnimatorController = gemManager.GetGemAnimController(gemType);
    }
}

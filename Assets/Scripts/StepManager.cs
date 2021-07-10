using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{
    [SerializeField] float mStepRate;
    GridManager mGridManager;
    GemManager mGemSpawnManager;
    Player mPlayer;
    BodyPart mSnakeHead;
    SnakeMovement mSnakeMovement;
    float mStepTimer;
    private void Awake()
    {
        mStepTimer = mStepRate;
        mGridManager = GetComponent<GridManager>();
        mGemSpawnManager = GetComponent<GemManager>();
    }
    void Start()
    {
        mPlayer = GameManager.Instance.GetPlayer();
        mSnakeMovement = mPlayer.GetComponent<SnakeMovement>();
        mSnakeHead = mPlayer.GetComponent<SnakeBodyManager>().GetHead();
    }

    void Update()
    {
        mStepTimer -= Time.deltaTime;

        if (mStepTimer <= 0)
        {
            Step();
            mStepTimer = mStepRate;
        }
    }
    void Step()
    {
        mGemSpawnManager.Step();
        mSnakeMovement.ApplyDirectionChange();

        GridManager.CellInfo targetCell = GetTargetCell();

        mGridManager.SteppedOnCell(targetCell);

        if (!GameManager.Instance.IsGameOver())
        {
            mSnakeMovement.MoveSnake(targetCell);
        }
    }

    GridManager.CellInfo GetTargetCell()
    {
        return mGridManager.GetCellAhead(mSnakeHead.GetCurCellIndex(), mSnakeHead.GetDirection());
    }
}

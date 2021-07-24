using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{
    public static float STEP_RATE = 0.3f;
    GridManager mGridManager;
    GemManager mGemSpawnManager;
    ComboManager mComboManager;
    SnakeBodyManager mBodyManager;
    Player mPlayer;
    BodyPart mSnakeHead;
    SnakeMovement mSnakeMovement;
    float mStepTimer;
    bool mPerformedComboLastStep;
    List<BodyPart> mComboListLastStep;
    private void Awake()
    {
        mStepTimer = STEP_RATE;
        mGridManager = GetComponent<GridManager>();
        mGemSpawnManager = GetComponent<GemManager>();
        mComboManager = GetComponent<ComboManager>();
    }
    void Start()
    {
        mPlayer = GameManager.Instance.GetPlayer();
        mSnakeMovement = mPlayer.GetComponent<SnakeMovement>();
        mBodyManager = mPlayer.GetComponent<SnakeBodyManager>();
        mSnakeHead = mBodyManager.GetHead();
    }

    void Update()
    {
        mStepTimer -= Time.deltaTime;

        if (mStepTimer <= 0)
        {
            Step();
            mStepTimer = STEP_RATE;
        }
    }
    void Step()
    {
        if (mPerformedComboLastStep)
        {
            mPerformedComboLastStep = false;
            foreach (BodyPart bodyPart in mComboListLastStep)
            {
                mBodyManager.RemoveBodyPart(bodyPart);
            }
            foreach (BodyPart bodyPart in mComboListLastStep)
            {
                mComboManager.DestroyBodyPart(bodyPart);
            }
            List<BodyPart> allBodyParts = mBodyManager.GetAllBodyParts();
            allBodyParts.RemoveAt(0);
            foreach (BodyPart bodyPart in allBodyParts)
            {
                mGridManager.MoveEntity(bodyPart.GetCurCellIndex(), mGridManager.GetCellAtPos(bodyPart.transform.position).index);
            }
            return;
        }
        mGemSpawnManager.Step();
        mSnakeMovement.ApplyDirectionChange();

        GridManager.CellInfo targetCell = GetTargetCell();

        mGridManager.SteppedOnCell(targetCell);

        if (!GameManager.Instance.IsGameOver())
        {
            mSnakeMovement.MoveSnake(targetCell);
            mComboManager.ComboCheck();
        }
    }

    GridManager.CellInfo GetTargetCell()
    {
        return mGridManager.GetCellAhead(mSnakeHead.GetCurCellIndex(), mSnakeHead.GetDirection());
    }
    public void PerformedCombo(List<BodyPart> comboList)
    {
        mPerformedComboLastStep = true;
        mComboListLastStep = comboList;
    }
    //Only used for debug
    public void ResetStepTimer()
    {
        mStepTimer = STEP_RATE;
    }
}

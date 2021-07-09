using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{
    [SerializeField] float stepRate;
    [SerializeField] GridManager gridManager;
    float stepTimer;
    Player player;
    BodyPart snakeHead;
    SnakeMovement snakeMovement;
    private void Awake()
    {
        stepTimer = stepRate;
    }
    void Start()
    {
        player = GameManager.Instance.GetPlayer();
        snakeMovement = player.GetComponent<SnakeMovement>();
        snakeHead = player.GetComponent<SnakeBodyManager>().GetHead();
    }

    void Update()
    {
        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0)
        {
            Step();
            stepTimer = stepRate;
        }
    }
    void Step()
    {
        snakeMovement.ApplyDirectionChange();

        GridManager.CellInfo targetCell = GetTargetCell();

        gridManager.SteppedOnCell(targetCell);

        if (!GameManager.Instance.IsGameOver())
        {
            snakeMovement.MoveSnake(targetCell);
        }
    }

    GridManager.CellInfo GetTargetCell()
    {
        return gridManager.GetCellAhead(snakeHead.GetCurCellIndex(), snakeHead.GetDirection());
    }
}

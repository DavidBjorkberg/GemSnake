using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    GameplayStatics.Direction mPendingDir;
    [SerializeField] SnakeBodyManager mSnakeBodyManager;
    BodyPart mHead;

    private void Start()
    {
        mHead = mSnakeBodyManager.GetHead();
    }

    void Update()
    {
        HandleMovementInput();
    }

    public void MoveSnake(GridManager.CellInfo targetCell)
    {
        mSnakeBodyManager.MoveSnake(targetCell);
    }
    void HandleMovementInput()
    {
#if UNITY_IOS || UNITY_ANDROID
        MobileInput(); 
#else
        DefaultInput();
#endif
    }
    bool TrySetPendingDir(GameplayStatics.Direction newPendingDir)
    {
        GameplayStatics.Direction facingDir = mHead.GetDirection();
        switch (newPendingDir)
        {
            case GameplayStatics.Direction.Up:
                if (facingDir == GameplayStatics.Direction.Down)
                {
                    return false;
                }
                break;
            case GameplayStatics.Direction.Right:
                if (facingDir == GameplayStatics.Direction.Left)
                {
                    return false;
                }
                break;
            case GameplayStatics.Direction.Down:
                if (facingDir == GameplayStatics.Direction.Up)
                {
                    return false;
                }
                break;
            case GameplayStatics.Direction.Left:
                if (facingDir == GameplayStatics.Direction.Right)
                {
                    return false;
                }
                break;
            default:
                break;
        }
        mPendingDir = newPendingDir;
        return true;
    }
    void DefaultInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            TrySetPendingDir(GameplayStatics.Direction.Up);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TrySetPendingDir(GameplayStatics.Direction.Right);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            TrySetPendingDir(GameplayStatics.Direction.Down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            TrySetPendingDir(GameplayStatics.Direction.Left);
        }
    }
    public void ApplyDirectionChange()
    {
       mHead.SetDirection(mPendingDir);
    }
    void MobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            float deltaX = touch.deltaPosition.x;
            float deltaY = touch.deltaPosition.y;
            if (deltaX == 0 && deltaY == 0)
            {
                return;
            }

            float deltaXAbs = Mathf.Abs(deltaX);
            float deltaYAbs = Mathf.Abs(deltaY);

            if (deltaXAbs > deltaYAbs)
            {
                if (deltaX > 0)
                {
                    TrySetPendingDir(GameplayStatics.Direction.Right);
                }
                else
                {
                    TrySetPendingDir(GameplayStatics.Direction.Left);
                }
            }
            else
            {
                if (deltaY > 0)
                {
                    TrySetPendingDir(GameplayStatics.Direction.Up);
                }
                else
                {
                    TrySetPendingDir(GameplayStatics.Direction.Down);
                }
            }

        }
    }


}

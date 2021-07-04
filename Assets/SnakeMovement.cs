using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    enum FacingDir { Up, Right, Down, Left };
    [SerializeField] FacingDir facingDir;
    [SerializeField] float stepRate;
    int stepSize = 1;
    float stepTimer;

    private void Awake()
    {
        stepTimer = stepRate;
    }
    void Start()
    {

    }

    void Update()
    {
        HandleMovementInput();
        StepUpdate();
    }
    void StepUpdate()
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
        switch (facingDir)
        {
            case FacingDir.Up:
                transform.position += Vector3.up * stepSize;
                break;
            case FacingDir.Right:
                transform.position += Vector3.right * stepSize;
                break;
            case FacingDir.Down:
                transform.position += Vector3.down * stepSize;
                break;
            case FacingDir.Left:
                transform.position += Vector3.left * stepSize;
                break;
            default:
                break;
        }
    }
    void HandleMovementInput()
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
                    TrySetFacingDir(FacingDir.Right);
                }
                else
                {
                    TrySetFacingDir(FacingDir.Left);
                }
            }
            else
            {
                if (deltaY > 0)
                {
                    TrySetFacingDir(FacingDir.Up);
                }
                else
                {
                    TrySetFacingDir(FacingDir.Down);
                }
            }

        }
    }
    bool TrySetFacingDir(FacingDir newFacingDir)
    {
        switch (newFacingDir)
        {
            case FacingDir.Up:
                if (facingDir == FacingDir.Down)
                {
                    return false;
                }
                break;
            case FacingDir.Right:
                if (facingDir == FacingDir.Left)
                {
                    return false;
                }
                break;
            case FacingDir.Down:
                if (facingDir == FacingDir.Up)
                {
                    return false;
                }
                break;
            case FacingDir.Left:
                if (facingDir == FacingDir.Right)
                {
                    return false;
                }
                break;
            default:
                break;
        }
        facingDir = newFacingDir;
        return true;
    }
}

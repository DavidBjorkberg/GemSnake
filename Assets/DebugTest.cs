using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    SnakeBodyManager mSnakebodyManager;
    GridManager mGridManager;
    [SerializeField] bool mDrawLinesBetweenSnakeParts;
    [SerializeField] bool mVisualizeGridEntities;
    void Start()
    {
        mSnakebodyManager = GameManager.Instance.GetPlayer().GetComponent<SnakeBodyManager>();
        mGridManager = GetComponent<GridManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ComboThreeMiddleBodies();
        }
        if (mDrawLinesBetweenSnakeParts)
        {
            DrawLinesBetweenSnakeParts();
        }
        if (mVisualizeGridEntities)
        {
            VisualizeGridEntities();
        }
    }
    void DrawLinesBetweenSnakeParts()
    {

    }
    void VisualizeGridEntities()
    {
        List<GameObject> visualGrid = mGridManager.GetVisualGrid();

        for (int i = 0; i < visualGrid.Count; i++)
        {
            if (mGridManager.GetCell(i).entity)
            {
                visualGrid[i].GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                visualGrid[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
    void ComboThreeMiddleBodies()
    {
        BodyPart head = mSnakebodyManager.GetHead();

        BodyPart middle = head.GetPreviousBodyPart().GetPreviousBodyPart();
        BodyPart prev = middle.GetPreviousBodyPart();
        BodyPart next = middle.GetNextBodyPart();
        List<BodyPart> comboList = new List<BodyPart>();
        comboList.Add(prev);
        comboList.Add(middle);
        comboList.Add(next);
        GetComponent<ComboManager>().ComboEnded(comboList);
        GetComponent<StepManager>().ResetStepTimer();
        //middle.SetIsPartOfCombo(true);
        //prev.SetIsPartOfCombo(true);
        //next.SetIsPartOfCombo(true);
        //GetComponent<StepManager>().PerformedCombo(comboList);
        //mSnakebodyManager.MoveBodyAfterCombo();

        GameManager.Instance.EndGame();
    }
    void DeleteThreeMiddleBodies()
    {
        BodyPart head = mSnakebodyManager.GetHead();

        BodyPart middle = head.GetPreviousBodyPart().GetPreviousBodyPart().GetPreviousBodyPart();
        BodyPart prev = middle.GetPreviousBodyPart();
        BodyPart next = middle.GetNextBodyPart();
        mSnakebodyManager.RemoveBodyPart(prev);
        mSnakebodyManager.RemoveBodyPart(middle);
        mSnakebodyManager.RemoveBodyPart(next);

        Destroy(middle.gameObject);
        Destroy(prev.gameObject);
        Destroy(next.gameObject);

    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying && mDrawLinesBetweenSnakeParts)
        {
            List<BodyPart> allBodyParts = mSnakebodyManager.GetAllBodyParts();

            for (int i = 1; i < allBodyParts.Count; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(allBodyParts[i].transform.position, allBodyParts[i - 1].transform.position);
            }
        }
    }
}

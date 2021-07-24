using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    [SerializeField] Player mPlayer;
    [SerializeField] GridManager mGridManager;
    StepManager mStepManager;
    GemManager mGemManager;
    bool mIsGameOver;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        mGemManager = GetComponent<GemManager>();
        mStepManager = GetComponent<StepManager>();
    }
    public Player GetPlayer()
    {
        return mPlayer;
    }
    public GridManager GetGridManager()
    {
        return mGridManager;
    }
    public GemManager GetGemManager()
    {
        return mGemManager;
    }
    public bool IsGameOver()
    {
        return mIsGameOver;
    }
    public void EndGame()
    {
        mIsGameOver = true;
        print("Ended game");
    }
    public StepManager GetStepManager()
    {
        return mStepManager;
    }
}

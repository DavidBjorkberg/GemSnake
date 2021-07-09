using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    [SerializeField] Player player;
    [SerializeField] GridManager gridManager;
    bool isGameOver;
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
    }
    void Start()
    {
    }
    void Update()
    {

    }


    public Player GetPlayer()
    {
        return player;
    }
    public GridManager GetGridManager()
    {
        return gridManager;
    }
    public bool IsGameOver()
    {
        return isGameOver;
    }
    public void EndGame()
    {
        isGameOver = true;
        print("Ended game");
    }
}

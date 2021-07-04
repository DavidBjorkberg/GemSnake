using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject snakeHead;
    [SerializeField] Vector2 playerSpawnPos;
    void Start()
    {
        SpawnPlayer();
    }
    void Update()
    {

    }

    void SpawnPlayer()
    {
        Instantiate(snakeHead, playerSpawnPos, Quaternion.identity);
    }
}

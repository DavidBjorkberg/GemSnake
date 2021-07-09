using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawnManager : MonoBehaviour
{
    [SerializeField] int gemSpawnDelayInSteps;
    [SerializeField] Gem gemPrefab;
    GridManager gridManager;
    int gemSpawnTimer;
    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }
    void Start()
    {
    }
    public void Step()
    {
        gemSpawnTimer--;
        if (gemSpawnTimer <= 0)
        {
            SpawnGem();
            gemSpawnTimer = gemSpawnDelayInSteps;
        }
    }
    void SpawnGem()
    {
        GridManager.CellInfo randomCell = gridManager.GetRandomFreeCell();

        Gem spawnedGem = Instantiate(gemPrefab, randomCell.pos, Quaternion.identity);
        gridManager.AddEntityToCell(spawnedGem, randomCell.index);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    public enum GemType { Ruby, Amber, Topaz, Emerald, Sapphire, Amethyst, Diamond, _MAX };
    [SerializeField] int gemSpawnDelayInSteps;
    [SerializeField] Gem gemPrefab;
    [SerializeField] List<RuntimeAnimatorController> animControllers;
    GridManager gridManager;
    int gemSpawnTimer;
    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
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
        spawnedGem.InitializeGem(GetGemTypeToSpawn());
        gridManager.AddEntityToCell(spawnedGem, randomCell.index);
    }
    public GemType GetGemTypeToSpawn()
    {
        int randomGem = Random.Range(0, (int)GemType._MAX);

        return (GemType)randomGem;
    }
    public RuntimeAnimatorController GetGemAnimController(GemType gemType)
    {
        return animControllers[(int)gemType];
    }
}

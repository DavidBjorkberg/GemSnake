using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    public enum GemType { Ruby, Amber, Topaz, Emerald, Sapphire, Amethyst, Diamond, _MAX };
    [SerializeField] int mGemSpawnDelayInSteps;
    [SerializeField] Gem mGemPrefab;
    [SerializeField] List<RuntimeAnimatorController> mAnimControllers;
    GridManager mGridManager;
    int mGemSpawnTimer;
    private void Awake()
    {
        mGridManager = GetComponent<GridManager>();
    }
    public void Step()
    {
        mGemSpawnTimer--;
        if (mGemSpawnTimer <= 0)
        {
            SpawnGem();
            mGemSpawnTimer = mGemSpawnDelayInSteps;
        }
    }
    void SpawnGem()
    {
        GridManager.CellInfo randomCell = mGridManager.GetRandomFreeCell();

        Gem spawnedGem = Instantiate(mGemPrefab, randomCell.pos, Quaternion.identity);
        spawnedGem.InitializeGem(GetGemTypeToSpawn());
        mGridManager.AddEntityToCell(spawnedGem, randomCell.index);
    }
    public GemType GetGemTypeToSpawn()
    {
        int randomGem = Random.Range(0, (int)GemType._MAX);

        return (GemType)randomGem;
    }
    public RuntimeAnimatorController GetGemAnimController(GemType gemType)
    {
        return mAnimControllers[(int)gemType];
    }
}

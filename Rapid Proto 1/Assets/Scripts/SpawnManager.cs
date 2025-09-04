using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Obstacles & Pickups")]
    public GameObject[] obstaclePrefabs;
    public GameObject healthPickupPrefab;
    public GameObject goldCoinPrefab;
    public GameObject silverCoinPrefab;

    public Transform player;
    public Transform finishLine;

    [Header("Spawning")]
    public float spawnAheadDistance = 50f;
    public float segmentLength = 10f;
    public float coinYOffset = 0.5f; // vain kolikoille

    private float nextSpawnX;
    private float[] lanes = { -4f, 0f, 4f };

    [Header("Scaling")]
    public int baseObstacleCount = 1;
    public int maxObstacleCount = 3;

    [Header("Pickup Chances")]
    [Range(0f, 1f)] public float healthPickupChance = 0.15f;
    [Range(0f, 1f)] public float goldCoinChance = 0.2f;
    [Range(0f, 1f)] public float silverCoinChance = 0.3f;

    void Start()
    {
        nextSpawnX = player.position.x - spawnAheadDistance;
    }

    void Update()
    {
        if (finishLine == null) return;

        while (nextSpawnX > finishLine.position.x)
        {
            SpawnObstaclesAndPickups();
            nextSpawnX -= segmentLength;
        }
    }

    void SpawnObstaclesAndPickups()
    {
        float progress = Mathf.Clamp01(-player.position.x / 1000f);

        // Obstacles
        int obstacleCount = Mathf.RoundToInt(Mathf.Lerp(baseObstacleCount, maxObstacleCount, progress));
        for (int i = 0; i < obstacleCount; i++)
        {
            SpawnOnGround(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], false);
        }

        // Health
        if (healthPickupPrefab != null && Random.value < healthPickupChance)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.currentHearts < playerHealth.maxHearts)
            {
                SpawnOnGround(healthPickupPrefab, false);
            }
        }

        // Gold Coin
        if (goldCoinPrefab != null && Random.value < goldCoinChance)
        {
            SpawnOnGround(goldCoinPrefab, true);
        }

        // Silver Coin
        if (silverCoinPrefab != null && Random.value < silverCoinChance)
        {
            SpawnOnGround(silverCoinPrefab, true);
        }
    }

    void SpawnOnGround(GameObject prefab, bool applyYOffset)
    {
        float laneZ = lanes[Random.Range(0, lanes.Length)];
        Vector3 spawnPos = new Vector3(nextSpawnX, 50f, laneZ);

        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 100f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                spawnPos.y = hit.point.y + (applyYOffset ? coinYOffset : 0f);
                Instantiate(prefab, spawnPos, Quaternion.identity);
            }
        }
    }
}

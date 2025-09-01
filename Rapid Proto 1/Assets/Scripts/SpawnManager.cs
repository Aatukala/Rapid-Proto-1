using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public GameObject healthPickupPrefab; // pickup
    public Transform player;
    public Transform finishLine;

    [Header("Spawning")]
    public float spawnAheadDistance = 50f;   // kuinka pitkälle eteenpäin spawnaa
    public float segmentLength = 10f;
    public float yOffset = 0.5f;

    private float nextSpawnX;
    private float[] lanes = { -4f, 0f, 4f };

    [Header("Scaling")]
    public int baseObstacleCount = 1;
    public int maxObstacleCount = 3;

    [Header("Pickups")]
    [Range(0f, 1f)] public float healthPickupChance = 0.15f; // 15% mahdollisuus segmentissä

    void Start()
    {
        // Asetetaan ensimmäinen spawn-piste vähän pelaajan eteen (negatiivinen x)
        nextSpawnX = player.position.x - spawnAheadDistance;
    }

    void Update()
    {
        if (finishLine == null) return;

        // Spawnaa esteitä ja pickuppeja niin kauan kuin ollaan ennen maaliviivaa
        while (nextSpawnX > finishLine.position.x)
        {
            SpawnObstaclesAndPickups();
            nextSpawnX -= segmentLength; // siirretään seuraava spawn-piste eteenpäin (-x suuntaan)
        }
    }

    void SpawnObstaclesAndPickups()
    {
        float progress = Mathf.Clamp01(-player.position.x / 1000f);

        int obstacleCount = Mathf.RoundToInt(Mathf.Lerp(baseObstacleCount, maxObstacleCount, progress));
        for (int i = 0; i < obstacleCount; i++)
        {
            SpawnOnGround(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
        }

        // Health pickup vain jos prefab on asetettu
        if (healthPickupPrefab != null && Random.value < healthPickupChance)
        {
            // tarkista pelaajan health ennen kuin spawnataan
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.currentHearts < playerHealth.maxHearts)
            {
                SpawnOnGround(healthPickupPrefab);
            }
        }
    }

    void SpawnOnGround(GameObject prefab)
    {
        float laneZ = lanes[Random.Range(0, lanes.Length)];
        Vector3 spawnPos = new Vector3(nextSpawnX, 50f, laneZ); // korkealle, raycast alas

        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 100f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                spawnPos.y = hit.point.y + yOffset;
                Instantiate(prefab, spawnPos, Quaternion.identity);
            }
        }
    }
}

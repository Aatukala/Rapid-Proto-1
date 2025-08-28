using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public Transform player;
    public float spawnAheadDistance = 50f; // Kuinka kaukana pelaajasta spawnaa
    public float segmentLength = 10f;      // Etäisyys spawnaussegmenttien välillä
    public float yOffset = 0.5f;

    private float nextSpawnX;
    private float[] lanes = { -4f, 0f, 4f };

    [Header("Scaling")]
    public int baseObstacleCount = 1;
    public int maxObstacleCount = 3;

    [Header("Progression")]
    public float maxDistance = 5000f; // ProgressBarin maksimietäisyys

    void Start()
    {
        // Pelaaja liikkuu -X, joten ensimmäinen spawn tulee edestä
        nextSpawnX = player.position.x - segmentLength;
    }

    void Update()
    {
        // Spawnataan niin kauan kuin pelaaja lähestyy nextSpawnX:ää
        while (player.position.x - spawnAheadDistance < nextSpawnX)
        {
            SpawnObstacles();
            nextSpawnX -= segmentLength; // Seuraava segmentti edemmäksi
        }
    }

    void SpawnObstacles()
    {
        float distanceTravelled = Mathf.Abs(player.position.x); 
        float progress = Mathf.Clamp(distanceTravelled / maxDistance, 0f, 1f);

        int obstacleCount = Mathf.RoundToInt(Mathf.Lerp(baseObstacleCount, maxObstacleCount, progress));

        for (int i = 0; i < obstacleCount; i++)
        {
            GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            float laneZ = lanes[Random.Range(0, lanes.Length)];
            Vector3 spawnPos = new Vector3(nextSpawnX, yOffset, laneZ);
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }
}

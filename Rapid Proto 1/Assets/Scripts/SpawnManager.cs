using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
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

    void Start()
    {
        // Asetetaan ensimmäinen spawn-piste vähän pelaajan eteen (negatiivinen x)
        nextSpawnX = player.position.x - spawnAheadDistance;
    }

    void Update()
    {
        if (finishLine == null) return;

        // Spawnaa esteitä niin kauan kuin ollaan ennen maaliviivaa
        while (nextSpawnX > finishLine.position.x)
        {
            SpawnObstacles();
            nextSpawnX -= segmentLength; // siirretään seuraava spawn-piste eteenpäin (-x suuntaan)
        }
    }

    void SpawnObstacles()
    {
        float progress = Mathf.Clamp01(-player.position.x / 1000f);

        int obstacleCount = Mathf.RoundToInt(Mathf.Lerp(baseObstacleCount, maxObstacleCount, progress));
        for (int i = 0; i < obstacleCount; i++)
        {
            GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            float laneZ = lanes[Random.Range(0, lanes.Length)];

            // Perus spawnipiste
            Vector3 spawnPos = new Vector3(nextSpawnX, 50f, laneZ); // laitetaan y korkealle

            // Raycast alaspäin löytämään maa
            if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 100f))
            {
                // Varmistetaan että osuimme vain maahan
                if (hit.collider.CompareTag("Ground"))
                {
                    spawnPos.y = hit.point.y + yOffset; 
                    Instantiate(prefab, spawnPos, Quaternion.identity);
                }
            }
        }
    }

}

using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // kivimuuri, rotko, kivipilari
    public Transform player;             // pelaajan sijainti
    public float spawnDistance = 30f;    // kuinka kaukana edessä este spawnaa
    public float segmentLength = 10f;    // väli seuraavaan spawniin
    
    private float nextSpawnZ = 0f;
    private float[] lanes = { -2f, 0f, 2f }; // kolme kaistaa

    void Update()
    {
        if (player.position.z > nextSpawnZ - spawnDistance)
        {
            SpawnObstacle();
            nextSpawnZ += segmentLength;
        }
    }

    void SpawnObstacle()
    {
        // Valitaan satunnainen este
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // Valitaan satunnainen kaista
        float laneX = lanes[Random.Range(0, lanes.Length)];

        // Luodaan este
        Vector3 spawnPos = new Vector3(laneX, 0, nextSpawnZ);
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}

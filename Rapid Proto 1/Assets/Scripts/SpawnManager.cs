using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // kivimuuri, rotko, kivipilari
    public Transform player;             // pelaajan sijainti
    public float spawnDistance = 30f;    // kuinka kaukana edessä este spawnaa
    public float segmentLength = 10f;    // väli seuraavaan spawniin

    private float nextSpawnX = 0f;       // koska liike X-suunnassa

    // Lane sijoittuu Z-akselille (vasen = -4, keskellä = 0, oikea = +4)
    private float[] lanes = { -4f, 0f, 4f };
    public float yOffset = 0.5f;

    void Update()
    {
        // Pelaaja liikkuu X-suunnassa eteenpäin
        if (player.position.x > nextSpawnX - spawnDistance)
        {
            SpawnObstacle();
            nextSpawnX += segmentLength;
        }
    }

    void SpawnObstacle()
    {
        // Satunnainen este
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // Satunnainen lane (Z-suunnassa)
        float laneZ = lanes[Random.Range(0, lanes.Length)];

        // Spawnataan este pelaajan eteen oikeaan laneen
        Vector3 spawnPos = new Vector3(nextSpawnX, yOffset, laneZ);
        Instantiate(prefab, spawnPos, Quaternion.identity);

        Debug.Log("Spawnattu: " + prefab.name + " paikkaan " + spawnPos);
    }
}

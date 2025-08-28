using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // kivimuuri, rotko, kivipilari
    public Transform player;
    public float spawnDistance = 50f;    // kuinka kaukana pelaajan edessä esteitä pitää olla
    public float segmentLength = 10f;    // väli seuraavien mahdollisten esteiden välillä
    
    private float nextSpawnX = 0f;

    // Kaistat Z-suunnassa
    private float[] lanes = { -4f, 0f, 4f };
    public float yOffset = 0.5f;

    void Start()
    {
        // aloitetaan spawnaus heti alun jälkeen (ettei pelaajan päälle tule)
        nextSpawnX = player.position.x + 15f;
    }

    void Update()
    {
        // generoidaan niin kauan kuin pelaajan eteen on tilaa
        while (nextSpawnX < player.position.x + spawnDistance)
        {
            SpawnObstacle();
            nextSpawnX += segmentLength;
        }
    }

    void SpawnObstacle()
    {
        // satunnaisesti jätetään osa väleistä tyhjäksi
        if (Random.value < 0.3f)
            return;

        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        float laneZ = lanes[Random.Range(0, lanes.Length)];

        Vector3 spawnPos = new Vector3(nextSpawnX, yOffset, laneZ);
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}

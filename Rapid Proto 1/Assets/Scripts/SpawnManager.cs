using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; 
    public Transform player;             
    public float spawnDistance = 30f;    
    public float segmentLength = 10f;    
    public float startDelay = 20f;       

    private float nextSpawnX;

    private float[] lanes = { -4f, 0f, 4f }; 
    public float yOffset = 0.5f;
    public LayerMask groundLayer; // Layer, jossa maa sijaitsee

    void Start()
    {
        nextSpawnX = startDelay;
    }

    void Update()
    {
        if (player.position.x > nextSpawnX - spawnDistance)
        {
            SpawnObstacle();
            nextSpawnX += segmentLength;
        }
    }

    void SpawnObstacle()
    {
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        float laneZ = lanes[Random.Range(0, lanes.Length)];

        // Spawnataan hieman korkeammalle, jotta raycast löytää maan
        Vector3 spawnPos = new Vector3(nextSpawnX, 10f, laneZ);

        // Raycast alas tarkistamaan, että maata löytyy
        RaycastHit hit;
        if (Physics.Raycast(spawnPos, Vector3.down, out hit, 20f, groundLayer))
        {
            // Maan päällä → spawnataan este
            spawnPos.y = hit.point.y + yOffset;
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
        else
        {
            // Rotko alla → ei spawnata
            // Debug.Log("Estettä ei spawndata: rotko alla!");
        }
    }
}

using UnityEngine;

public enum ObstacleType
{
    Wall,   // kivimuuri
    Pit,    // rotko
    Pillar  // kivipilari
}


public class Obstacle : MonoBehaviour
{

    public ObstacleType type;

    private void OnDrawGizmos()
    {
        Gizmos.color = type switch
        {
            ObstacleType.Wall => Color.red,
            ObstacleType.Pit => Color.blue,
            ObstacleType.Pillar => Color.green,
            _ => Color.white
        };
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }
}




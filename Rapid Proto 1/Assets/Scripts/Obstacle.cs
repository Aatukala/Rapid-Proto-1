using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType
{
    Wall,   // kivimuuri → pitää hypätä yli
    Pit,    // rotko → pitää hypätä yli
    Pillar  // kivipilari → pitää väistää kaistaa vaihtamalla
}

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
}

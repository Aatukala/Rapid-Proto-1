using UnityEngine;

public class Coin : MonoBehaviour
{
    public int points = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            ScoreManager.Instance.AddScore(points);
            Destroy(gameObject); 
        }
    }
}

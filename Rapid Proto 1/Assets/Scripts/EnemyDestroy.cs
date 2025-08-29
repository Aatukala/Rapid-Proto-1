using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spear"))
        {
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

            Destroy(collision.gameObject); 
            Destroy(gameObject);          
        }
    }
}

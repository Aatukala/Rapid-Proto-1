using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(healAmount);
            }

            Destroy(gameObject); // poista pickup keräämisen jälkeen
        }
    }
}

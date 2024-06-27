using Unity.Netcode;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rigidBody = other.attachedRigidbody;
        if (rigidBody == null)
        {
            return;
        }
        if (rigidBody.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
    }
}

using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 30f;
    private Vector3 targetPosition;
    private float lifetime = 0.5f; // Time before the arrow is destroyed
    private float timer;


    public void Initialize(Vector3 target)
    {
        targetPosition = target;

        // Set rotation to face the target
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
        // Set the arrow's lifetime
        timer = lifetime;
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject); // Destroy the arrow after its lifetime
        }

    }
}

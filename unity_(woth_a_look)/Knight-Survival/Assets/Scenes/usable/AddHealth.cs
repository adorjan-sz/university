using UnityEngine;

public class AddHealth : MonoBehaviour
{
    public float rotationSpeed = 50f;

    void Update()
    {
        // Rotate around Y axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        SetPickedUpAndDestroy(other);
    }

    private void SetPickedUpAndDestroy(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player health increased");
            EntityManager.Instance.GetPlayer().pickedUpHealth = true;
            Destroy(gameObject);
        }
    }
}

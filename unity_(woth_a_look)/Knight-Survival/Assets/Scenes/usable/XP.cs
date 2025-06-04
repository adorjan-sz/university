using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public bool pickedUp = false;

    void Update()
    {
        // Rotate around Y axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        DestoryIfPickedUp();
    }

    private void DestoryIfPickedUp()
    {
        if (pickedUp)
        {
            Debug.Log("Player collected XP orb!");
            Player player = EntityManager.Instance.GetPlayer();
            player.xp += 0.05f;
            if (player.xp >= player.xpToLevelUp)
            {
                player.levelUp = true;
            }
            player.pickup.Play();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickedUp = true;
        }
    }
}

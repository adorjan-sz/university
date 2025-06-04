using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public GameObject xpOrbPrefab;
    public GameObject AddHealth;

    public float speed = 3f;
    public float attackRange = 2f;
    public float Health;

    private float hitTimer = 2f;
    


    void Start()
    {
        EntityManager.Instance.RegisterEnemy(this);
        Health = 7;
        
    }

    void OnDestroy()
    {
        Debug.Log("Destroy Called");
        if (EntityManager.Instance != null)
            EntityManager.Instance.UnregisterEnemy(this);
    }
    void OnTriggerEnter(Collider other)
    {
        HandleHittingPlayerSword(other);
        HandleHitByPlayerArrow(other);
    }

    private void HandleHitByPlayerArrow(Collider other)
    {
        if (other.CompareTag("playerArrow") && Health > 0)
        {
            Debug.Log("Hit by player ARROW!");
            Health -= 1 * EntityManager.Instance.PlayerDamage;
            IsDead();
        }
    }

    private void HandleHittingPlayerSword(Collider other)
    {
        if (other.CompareTag("PlayerSword") && Health > 0)
        {
            Debug.Log("Hit by player sword!");
            Health -= 2 * EntityManager.Instance.PlayerDamage;
            EntityManager.Instance.GetPlayer().sword.Play();
            IsDead();
        }
    }

    protected void IsDead()
    {
        if (Health <= 0)
        {
            animator.SetTrigger("Dead");
            gameObject.GetComponent<Collider>().enabled = false; // Disable collider to prevent further hits
                                                                 // Spawn XP orb at enemy position
            SpawnXP();
            SpawnHealth();

            Destroy(gameObject, 2f); // Delay destruction to allow death animation to play
        }
    }

    private void SpawnHealth()
    {
        if (AddHealth != null)
        {
            //random chance to spawn health
            float randomValue = Random.Range(0f, 1f);
            if (randomValue < 0.4f)
            {
                Instantiate(AddHealth, transform.position + Vector3.up * 0.5f + Vector3.right * 0.5f, Quaternion.identity);
            }
        }
    }

    private void SpawnXP()
    {
        if (xpOrbPrefab != null)
        {
            Instantiate(xpOrbPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }

    void Update()
    {
        if (player == null) return;
        if (Health <= 0) return; // Don't update if dead
                                 // Rotate to face the player
        Vector3 lookDirection = LookTowardsPlayer();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            ChasePlayer(lookDirection);
        }
        hitTimer -= Time.deltaTime;
        // Try to attack only if close enough
        if (distanceToPlayer <= attackRange)
        {
            if (hitTimer <= 0)
            {
                AttackPlayer();
            }
            else
            {
                PrepareAttack();
            }
        }
    }

    private void PrepareAttack()
    {
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.radius = 1;
        collider.height = 1;
    }

    private void AttackPlayer()
    {
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.radius = 3;
        collider.height = 1;
        animator.SetTrigger("Attack");
        hitTimer = 2f;
    }

    private void ChasePlayer(Vector3 lookDirection)
    {
        // Chase the player
        Vector3 direction = lookDirection.normalized;
        transform.position += direction * speed * Time.deltaTime;
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.radius = 1;
        collider.height = 1;
    }

    private Vector3 LookTowardsPlayer()
    {
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0; // Optional: Keep enemy upright, ignore vertical rotation
        transform.rotation = Quaternion.LookRotation(lookDirection);
        return lookDirection;
    }
}

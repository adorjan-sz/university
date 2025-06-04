using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    //create a signal
    public event Action<List<PlayerPerks>> LevelUp;
    public event Action Died;

    public CharacterController controller;
    public GameObject arrowPrefab;
    
    //hand slots
    public Transform handSlotRight;
    public Transform handSlotLeft;

    //usable items
    public GameObject basicSword;
    public GameObject UpgradedSword;
    
    public List<PlayerPerks> perks = new List<PlayerPerks>();

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
    //sword
    private float hitTimer = 0f;
    public float hitInterval = 4f;
    //arrow
    private float arrowTimer = 0f;
    public float arrowInterval = 2f;


    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Animator animator;

    public float Health;
    public bool pickedUpHealth = false;
    public float xp;
    public float xpToLevelUp = 1f;
    public bool levelUp = false;
    Vector3 velocity;

    bool isGrounded;


    public AudioSource pickup;
    public AudioSource hit;
    public AudioSource arrowSfx;
    public AudioSource levelup;
    public AudioSource walking;
    public AudioSource sword;
    void Start()
    {
        InstantiatePlayer();
    }

    private void InstantiatePlayer()
    {
        Health = 100;
        xp = 0;
        GameObject sword = Instantiate(basicSword, handSlotRight);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;
    }

    private void OnTriggerStay(Collider other)
    {
        HandleCollisionWithEnemyHitBox(other);
    }

    private void HandleCollisionWithEnemyHitBox(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit by enemy!");
            hit.Play();
            Health -= 1;
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (Health <= 0)
        {
            animator.SetTrigger("Dead");
            gameObject.GetComponent<Collider>().enabled = false; // Disable collider to prevent further hits
            //Destroy(gameObject, 2f);                            // Delay destruction to allow death animation to play
            Died.Invoke();
        }
    }

    public void UpgradeSword()
    {
        //delete current sword
        Destroy(handSlotRight.GetChild(0).gameObject);
        //create new sword
        GameObject sword = Instantiate(UpgradedSword, handSlotRight);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;
    }

    void ShootArrowAtNearestEnemy()
    {
        Enemy nearest = null;
        FindNearestEnemy(ref nearest);

        if (nearest != null)
        {
            //player current coord
            Debug.Log("Shooting arrow at nearest enemy");
            Vector3 playerPosition = transform.position;
            GameObject arrow = Instantiate(arrowPrefab, playerPosition, Quaternion.identity);


            // Ignore collision between arrow and player
            DisableCollisions(arrow);
            Vector3 finalDir = CalculateArrowDirection(nearest);
            // Initialize arrow
            arrow.GetComponent<Arrow>().Initialize(finalDir);
            arrowSfx.Play();
        }
    }

    private void DisableCollisions(GameObject arrow)
    {
        Collider arrowCollider = arrow.GetComponent<Collider>();
        Collider playerCollider = GetComponent<Collider>();
        if (arrowCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(arrowCollider, playerCollider);
        }
    }

    private static Vector3 CalculateArrowDirection(Enemy nearest)
    {
        CapsuleCollider collider = nearest.GetComponent<CapsuleCollider>();
        float height = collider.height / 2;
        float finalPos = nearest.transform.position.y + height;
        Vector3 finalDir = new Vector3(nearest.transform.position.x, finalPos, nearest.transform.position.z);
        return finalDir;
    }

    private void FindNearestEnemy(ref Enemy nearest)
    {
        float minDistance = Mathf.Infinity;
        foreach (Enemy e in EntityManager.Instance.GetEnemies())
        {
            CheckEnemyDistance(ref nearest, ref minDistance, e);
        }
    }

    private void CheckEnemyDistance(ref Enemy nearest, ref float minDistance, Enemy e)
    {
        if (e.Health == 0)
        {
            return;
        }
        float dist = Vector3.Distance(transform.position, e.transform.position);
        if (dist < minDistance)
        {
            minDistance = dist;
            nearest = e;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0) return; // Don't update if dead
        HandleLevelUp();
        HandleHealthPickedUp();
        HandleGravity();
        HandleMovement();
        HandleAttacking();
        HandleArrowShooting();
    }

    private void HandleArrowShooting()
    {
        arrowTimer += Time.deltaTime;
        ShootArrow();
    }

    private void ShootArrow()
    {
        if (arrowTimer >= arrowInterval)
        {
            for (int i = 0; i < EntityManager.Instance.arrows; i++)
            {
                ShootArrowAtNearestEnemy();
                Debug.Log("Shooting arrow");
            }

            arrowTimer = 0f;
        }
    }

    private void HandleAttacking()
    {
        hitTimer += Time.deltaTime;
        PlayAttackAnimation();
    }

    private void PlayAttackAnimation()
    {
        if (hitTimer >= hitInterval)
        {
            gameObject.GetComponent<Collider>().enabled = true;
            animator.SetTrigger("Hit");
            hitTimer = 0f;
        }
    }

    private void HandleMovement()
    {
        float x, z;
        MoveCharacter(out x, out z);
        float Movespeed = ScaleAnimationSpeed(x, z);
        PlayMovementSounds(Movespeed);
    }

    private float ScaleAnimationSpeed(float x, float z)
    {
        Vector3 direction = new Vector3(x, 0f, z);
        // Update animation based on movement magnitude
        float Movespeed = direction.magnitude;
        animator.SetFloat("Speed", Movespeed);
        return Movespeed;
    }

    private void MoveCharacter(out float x, out float z)
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        //right is the red Axis, foward is the blue axis
        Vector3 move = new Vector3(x, 0f, z);
        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        if (isActiveAndEnabled)
        {
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void PlayMovementSounds(float Movespeed)
    {
        if (Movespeed > 0.1 && Time.timeScale > 0)
        {
            if (!walking.isPlaying)
            {
                walking.loop = true;
                walking.Play();
            }
        }
        else
        {
            walking.Stop();
        }
    }

    private void HandleGravity()
    {
        //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void HandleHealthPickedUp()
    {
        if (pickedUpHealth)
        {
            Health = Math.Min(100 * EntityManager.Instance.PlayerHealth, Health + 10);
            pickedUpHealth = false;
        }
    }

    private void HandleLevelUp()
    {
        if (levelUp)
        {
            levelup.Play();
            xp = 0;
            xpToLevelUp *= 2;
            Debug.Log("Level Up!");
            LevelUp?.Invoke(perks);
            Time.timeScale = 0;
            levelUp = false;
        }
    }
}

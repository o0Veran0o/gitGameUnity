using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int attackDamage = 10; // Damage dealt to the player

    [SerializeField] Transform target;
    [SerializeField] float speed = 1.5f;
    [SerializeField] float attackRange = 200.0f; // Range within which the enemy attacks
    public float attackCooldown = 1.0f; // Attack every 1 second
    private float nextAttackTime = 0f;
    NavMeshAgent agent;
    [SerializeField] private Transform respawnPoint; // Assign in the Inspector

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Or set to false if you want physics based collisions
        }

        // DON'T ignore collisions with the player anymore. We want collision to trigger the attack
    }


    private void Update()
    {
        agent.SetDestination(target.position);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning("Player Health component not found!");
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Get the Player's Health component
        PlayerStats playerHealth = target.GetComponent<PlayerStats>();

        // If the Player has a Health component, deal damage
        if (playerHealth != null)
        {
            playerHealth.TakeDmg(attackDamage);
        }
        else
        {
            Debug.LogWarning("Player Health component not found!");
        }



    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time > nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown; // Set the next attack time
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Respawn instead of destroying
        Respawn();
    }
    private void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            health = 100; // Reset health (or any other relevant stats)

            // Reset NavMeshAgent if necessary (can prevent issues after teleporting)
            agent.Warp(respawnPoint.position);

            // Add any other respawn logic (e.g., reset animation, invincibility frames, etc.)

        }
        else
        {
            Debug.LogError("Respawn point not assigned!");
            Destroy(gameObject); // Fallback if no respawn point is set
        }
    }


}
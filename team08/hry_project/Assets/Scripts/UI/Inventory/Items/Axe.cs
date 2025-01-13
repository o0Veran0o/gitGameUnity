using UnityEngine;

public class Axe : Item
{
    public int damage = 20;
    public float attackRange = 120f;
    public Transform attackPoint;

    private PlayerStats playerStats; // Store a reference to player stats


    void Awake()
    {
        m_ItemName = "Axe";
        m_MaxStack = 1;
        m_IsConsumable = false;
    }

    public override void UseItem(PlayerStats player)
    {
        playerStats = player; // Assign the player stats when UseItem is called

        Attack(); // Call the Attack method
    }


    public void Attack()
    {
        if (attackPoint == null)
        {
            attackPoint = transform; // Fallback if attackPoint is not set
        }

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (Collider2D hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                // Apply knockback (if playerStats is available)
                if (playerStats != null)
                {
                    Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                        enemyRb.AddForce(knockbackDirection *2, ForceMode2D.Impulse);
                    }
                }

                break; // Only damage the first enemy (or remove to damage all)
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void ActivateForUse(Transform parent)
    {
        gameObject.SetActive(true);
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Get the attackPoint after parenting (if it's a child of the axe)
        if (attackPoint == null)
            attackPoint = transform.Find("AttackPoint"); // Replace "AttackPoint" with the actual name of your attack point object


    }
}
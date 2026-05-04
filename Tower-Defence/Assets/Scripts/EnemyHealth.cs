using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;

    private int currentHealth;
    private EnemySpawner spawner;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (spawner != null)
        {
            spawner.EnemyFinished();
        }

        Destroy(gameObject);
    }
}

using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int goldReward = 3;

    private int currentHealth;
    private EnemySpawner spawner;
    private GoldManager goldManager;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        goldManager = FindFirstObjectByType<GoldManager>();
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

        if (goldManager != null)
        {
            goldManager.AddGold(goldReward);
        }

        if (spawner != null)
        {
            spawner.EnemyFinished();
        }

        Destroy(gameObject);
    }
}
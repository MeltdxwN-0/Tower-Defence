using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Stats")]
    [SerializeField] private float range = 4f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int damage = 1;

    [Header("Upgrades")]
    [SerializeField] private int damageUpgradeAmount = 1;
    [SerializeField] private float rangeUpgradeAmount = 0.5f;
    [SerializeField] private float fireRateUpgradeAmount = 0.25f;

    private float fireCooldown = 0f;
    private Transform target;
    private TowerRangeVisual rangeVisual;

    public float Range => range;
    public float FireRate => fireRate;
    public int Damage => damage;

    private void Awake()
    {
        rangeVisual = GetComponent<TowerRangeVisual>();

        if (rangeVisual != null)
        {
            rangeVisual.SetRange(range);
            rangeVisual.Hide();
        }
    }

    private void Update()
    {
        FindTarget();

        if (target == null)
            return;

        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }
    }

    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void Shoot()
    {
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }

    public void ShowRange()
    {
        if (rangeVisual != null)
        {
            rangeVisual.SetRange(range);
            rangeVisual.Show();
        }
    }

    public void HideRange()
    {
        if (rangeVisual != null)
        {
            rangeVisual.Hide();
        }
    }

    public void UpgradeDamage()
    {
        damage += damageUpgradeAmount;
        Debug.Log("Damage upgraded to: " + damage);
    }

    public void UpgradeRange()
    {
        range += rangeUpgradeAmount;

        if (rangeVisual != null)
            rangeVisual.SetRange(range);

        Debug.Log("Range upgraded to: " + range);
    }

    public void UpgradeFireRate()
    {
        fireRate += fireRateUpgradeAmount;
        Debug.Log("Fire rate upgraded to: " + fireRate);
    }
}
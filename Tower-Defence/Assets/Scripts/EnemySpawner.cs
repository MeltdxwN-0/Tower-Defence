using UnityEngine;
using System.Collections;

//for å styre hvor mange fiender og i hvilken hastighet de kommer
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform pathParent;
    [SerializeField] private float timeBetweenSpawns = 1f;
    [SerializeField] private int enemyCount = 10;

    private Transform[] waypoints;

    private void Start()
    {
        waypoints = new Transform[pathParent.childCount];

        for (int i = 0; i < pathParent.childCount; i++)
        {
            waypoints[i] = pathParent.GetChild(i);
        }

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
    // for å se hva fienden skal gjøre etter den har spawnet 
    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(
            enemyPrefab,
            waypoints[0].position,
            Quaternion.identity
        );

        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        movement.SetWaypoints(waypoints);
    }
}


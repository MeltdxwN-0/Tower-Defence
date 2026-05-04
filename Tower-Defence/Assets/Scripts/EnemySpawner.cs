using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class EnemyGroup
{
    public GameObject enemyPrefab;
    public int enemyCount = 10;
    public float timeBetweenSpawns = 1f;
}

[System.Serializable]
public class Wave
{
    public EnemyGroup[] enemyGroups;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] private Transform pathParent;

    [Header("Waves")]
    [SerializeField] private Wave[] waves;

    [Header("UI")]
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private Button nextWaveButton;

    private Transform[] waypoints;
    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;
    private bool waitingForNextWave = true;

    private void Start()
    {
        SetupWaypoints();
        SetupNextWaveButton();
        UpdateWaveUI();
    }

    private void SetupWaypoints()
    {
        waypoints = new Transform[pathParent.childCount];

        for (int i = 0; i < pathParent.childCount; i++)
        {
            waypoints[i] = pathParent.GetChild(i);
        }
    }

    private void SetupNextWaveButton()
    {
        if (nextWaveButton != null)
        {
            nextWaveButton.onClick.AddListener(StartNextWave);
            nextWaveButton.gameObject.SetActive(false);
            nextWaveButton.GetComponentInChildren<TMP_Text>().text = "Next";
        }
    }

    public void StartNextWave()
    {
        if (isSpawning)
            return;

        if (currentWaveIndex >= waves.Length)
            return;

        waitingForNextWave = false;

        if (nextWaveButton != null)
            nextWaveButton.gameObject.SetActive(false);

        Time.timeScale = 1f;

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        isSpawning = true;

        Wave currentWave = waves[currentWaveIndex];
        UpdateWaveUI();

        foreach (EnemyGroup group in currentWave.enemyGroups)
        {
            for (int i = 0; i < group.enemyCount; i++)
            {
                SpawnEnemy(group.enemyPrefab);
                yield return new WaitForSeconds(group.timeBetweenSpawns);
            }
        }

        isSpawning = false;

        while (enemiesAlive > 0)
        {
            yield return null;
        }

        currentWaveIndex++;

        if (currentWaveIndex < waves.Length)
        {
            PauseBetweenWaves();
        }
        else
        {
            Victory();
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        enemiesAlive++;

        GameObject enemy = Instantiate(
            enemyPrefab,
            waypoints[0].position,
            Quaternion.identity
        );

        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

        if (movement != null)
        {
            movement.SetWaypoints(waypoints);
            movement.SetSpawner(this);
        }

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();

        if (health != null)
        {
            health.SetSpawner(this);
        }
    }

    public void EnemyFinished()
    {
        enemiesAlive--;

        if (enemiesAlive < 0)
        {
            enemiesAlive = 0;
        }
    }

    private void PauseBetweenWaves()
    {
        waitingForNextWave = true;
        Time.timeScale = 0f;

        if (nextWaveButton != null)
        {
            nextWaveButton.gameObject.SetActive(true);
        }

        UpdateWaveUI();
    }

    private void Victory()
    {
        waitingForNextWave = false;
        Time.timeScale = 0f;

        if (nextWaveButton != null)
        {
            nextWaveButton.gameObject.SetActive(false);
        }

        UpdateWaveUI();

        Debug.Log("Victory");
    }

    private void UpdateWaveUI()
    {
        if (waveText == null)
            return;

        if (currentWaveIndex >= waves.Length)
        {
            waveText.text = "Victory";
        }
        else if (waitingForNextWave)
        {
            waveText.text = "Klar for wave " + (currentWaveIndex + 1) + " / " + waves.Length;
        }
        else
        {
            waveText.text = "Wave: " + (currentWaveIndex + 1) + " / " + waves.Length;
        }
    }
}
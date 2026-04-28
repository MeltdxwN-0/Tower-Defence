using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isPaused = true;
    private bool hasStarted = false;
    private bool isGameOver = false;

    private void Start()
    {
        Time.timeScale = 1f;

        startButton.onClick.AddListener(StartGame);
        pauseButton.onClick.AddListener(TogglePause);
        restartButton.onClick.AddListener(RestartLevel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        startButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        PauseGame();
    }

    private void StartGame()
    {
        if (isGameOver)
            return;

        hasStarted = true;
        ResumeGame();

        startButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);

        if (enemySpawner != null)
        {
            enemySpawner.StartNextWave();
        }
    }

    private void TogglePause()
    {
        if (!hasStarted || isGameOver)
            return;

        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;

        if (pauseButton != null)
            pauseButton.GetComponentInChildren<TMP_Text>().text = "Resume";
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseButton != null)
            pauseButton.GetComponentInChildren<TMP_Text>().text = "Pause";
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(false);

        if (startButton != null)
            startButton.gameObject.SetActive(false);
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}

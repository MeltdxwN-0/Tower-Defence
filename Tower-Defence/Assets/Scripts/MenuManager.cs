using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startMenuCanvas;
    public GameObject optionMenuCanvas;
    public GameObject homeMenuCanvas;
    public GameObject levelSelectCanvas;

    public void ShowStartMenu()
    {
        startMenuCanvas.SetActive(true);
        optionMenuCanvas.SetActive(false);
        homeMenuCanvas.SetActive(false);
        levelSelectCanvas.SetActive(false);
    }

    public void ShowOptionsMenu()
    {
        optionMenuCanvas.SetActive(true);
    }

    public void HideOptionsMenu()
    {
        optionMenuCanvas.SetActive(false);
    }

    public void ShowHomeMenu()
    {
        startMenuCanvas.SetActive(false);
        homeMenuCanvas.SetActive(true);
        levelSelectCanvas.SetActive(false);
    }

    public void ShowLevelSelectMenu()
    {
        homeMenuCanvas.SetActive(false);
        levelSelectCanvas.SetActive(true);
    }

    public void BackToHomeMenu()
    {
        levelSelectCanvas.SetActive(false);
        homeMenuCanvas.SetActive(true);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
}
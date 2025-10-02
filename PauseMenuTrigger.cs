using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuTrigger : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    public void ShowPauseMenu()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;

        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        
        pauseMenuUI.SetActive(false);
    }

    public void Restart()
    {
        StartCoroutine(RestartDelayed());
    }

    public void Exit()
    {
        StartCoroutine(ExitDelayed());
    }

    private IEnumerator RestartDelayed()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator ExitDelayed()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenu");
    }
}
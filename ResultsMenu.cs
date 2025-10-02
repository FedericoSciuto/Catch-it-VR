using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResultsMenu : MonoBehaviour
{
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
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject modeCanvas;
    public GameObject grabLevelsCanvas;
    public GameObject hitLevelsCanvas;
    public GameObject optionsCanvas;
    public GameObject statsCanvas;
    public GameObject levelStatsCanvas;
    public GameObject userStatsCanvas;
    public GameObject userCanvas;
    public PlayableDirector startLevel1GrabDirector;
    public PlayableDirector startLevel2GrabDirector;
    public PlayableDirector startLevel1HitDirector;
    public PlayableDirector startLevel2HitDirector;

    private int selectedLevelId;
    private string selectedUsername;
    
    public void ShowModeSelection()
    {
        StartCoroutine(ShowModeSelectionDelayed());
    }

    public void ShowOptions()
    {
        StartCoroutine(ShowOptionsDelayed());
    }

    public void ShowStats()
    {
        StartCoroutine(ShowStatsDelayed());
    }

    public void QuitApplication()
    {
        StartCoroutine(QuitApplicationDelayed());
    }

    public void ShowUserSelection()
    {
        StartCoroutine(ShowUserSelectionDelayed());
    }

    public void BackToMainMenu()
    {
        StartCoroutine(BackToMainMenuDelayed());
    }

    public void ShowGrabLevelsSelection()
    {
        StartCoroutine(ShowGrabLevelsSelectionDelayed());
    }

    public void ShowHitLevelsSelection()
    {
        StartCoroutine(ShowHitLevelsSelectionDelayed());
    }

    public void BackToModeSelection()
    {
        StartCoroutine(BackToModeSelectionDelayed());
    }

    public void StartLevel1Grab()
    {
        startLevel1GrabDirector.Play();
    }

    public void StartLevel2Grab()
    {
        startLevel2GrabDirector.Play();
    }

    public void StartLevel1Hit()
    {
        startLevel1HitDirector.Play();
    }

    public void StartLevel2Hit()
    {
        startLevel2HitDirector.Play();
    }

    public void BackToStats()
    {
        StartCoroutine(BackToStatsDelayed());
    }

    public void ShowLevelStats(int levelId)
    {
        selectedLevelId = levelId;
        StartCoroutine(ShowLevelStatsDelayed());
    }

    public void BackToLevelStats()
    {
        StartCoroutine(BackToLevelStatsDelayed());
    }

    public void ShowUserStats(Button clickedButton)
    {
        var textComponent = clickedButton.GetComponentInChildren<TMP_Text>();
        string fullText = textComponent.text;
        int separatorIndex = fullText.IndexOf("  |");
        selectedUsername = fullText.Substring(0, separatorIndex);
        
        StartCoroutine(ShowUserStatsDelayed());
    }

    private IEnumerator ShowModeSelectionDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        modeCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }

    private IEnumerator ShowOptionsDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        optionsCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }

    private IEnumerator ShowStatsDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        statsCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }

    private IEnumerator QuitApplicationDelayed()
    {
        yield return new WaitForSeconds(0.5f);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private IEnumerator ShowUserSelectionDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        userCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }

    private IEnumerator BackToMainMenuDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        mainMenuCanvas.SetActive(true);
        modeCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        statsCanvas.SetActive(false);
        userCanvas.SetActive(false);
    }

    private IEnumerator ShowGrabLevelsSelectionDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        grabLevelsCanvas.SetActive(true);
        modeCanvas.SetActive(false);
    }

    private IEnumerator ShowHitLevelsSelectionDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        hitLevelsCanvas.SetActive(true);
        modeCanvas.SetActive(false);
    }

    private IEnumerator BackToModeSelectionDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        modeCanvas.SetActive(true);
        grabLevelsCanvas.SetActive(false);
        hitLevelsCanvas.SetActive(false);
    }

    private IEnumerator BackToStatsDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        statsCanvas.SetActive(true);
        levelStatsCanvas.SetActive(false);
    }

    private IEnumerator ShowLevelStatsDelayed()
    {
        StatsManager.Instance.SetSelectedLevel(selectedLevelId);
        StatsManager.Instance.LoadLevelStats();
        yield return new WaitForSeconds(0.1f);
        levelStatsCanvas.SetActive(true);
        statsCanvas.SetActive(false);
    }

    private IEnumerator BackToLevelStatsDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        levelStatsCanvas.SetActive(true);
        userStatsCanvas.SetActive(false);
    }

    private IEnumerator ShowUserStatsDelayed()
    {
        StatsManager.Instance.LoadUserStats(selectedUsername);
        yield return new WaitForSeconds(0.1f);
        userStatsCanvas.SetActive(true);
        levelStatsCanvas.SetActive(false);
    }
}
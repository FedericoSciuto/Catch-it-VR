using UnityEngine;
using TMPro;
using System.Collections;

public class UserSelector : MonoBehaviour
{
    public string username;
    public TextMeshProUGUI statusText;
    public GameObject mainMenuCanvas;
    public GameObject usersCanvas;

    public void SelectUser()
    {
        if (string.IsNullOrEmpty(username))
        {
            UserManager.Instance.ClearUser();
            statusText.text = "Seleziona utente";
        }
        else if (UserManager.Instance.SetUser(username))
        {
            statusText.text = $"Utente selezionato: {username}";
        }

        StartCoroutine(BackToMainMenuDelayed());
    }

    private IEnumerator BackToMainMenuDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        mainMenuCanvas.SetActive(true);
        usersCanvas.SetActive(false);
    }
}

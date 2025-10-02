using UnityEngine;
using TMPro;

public class UserStatusUI : MonoBehaviour
{
    public TextMeshProUGUI statusText;

    void Start()
    {
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        if (UserManager.Instance != null && UserManager.Instance.HasUser())
        {
            statusText.text = $"Utente selezionato: {UserManager.Instance.SelectedUsername}";
        }
        else
        {
            statusText.text = "Seleziona utente";
        }
    }
}

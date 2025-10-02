using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance;

    public string SelectedUsername { get; private set; } = "";
    public string Address { get; private set; } = "";
    public string PrivateKey { get; private set; } = "";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool SetUser(string username)
    {
        if (UserAccounts.accountMap.TryGetValue(username, out var info))
        {
            SelectedUsername = username;
            Address = info.address;
            PrivateKey = info.privateKey;
            Debug.Log($"Utente selezionato: {username} - {Address}");
            return true;
        }

        Debug.LogWarning($"Username non valido: {username}");
        return false;
    }

    public void ClearUser()
    {
        SelectedUsername = "";
        Address = "";
        PrivateKey = "";
    }

    public bool HasUser() => !string.IsNullOrEmpty(SelectedUsername);
}
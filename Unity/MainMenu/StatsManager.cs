using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.Web3;
using Nethereum.ABI.FunctionEncoding.Attributes;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    public TMP_Text textLevelStats;
    public Button[] usersButtons;
    public TMP_Text textUserStatsTitle;
    public TMP_Text textUserStatsLevel;
    public Button[] statsButtons;

    private int selectedLevelId;

    private string contractAddress = "0x60C92C0E63E4A2070dd96ACCd7DD1e906484CaE0";
    private string rpcUrl = "http://192.168.1.102:7545";
    private string abi = @"[
        {
            ""anonymous"": false,
            ""inputs"": [
                {
                    ""indexed"": true,
                    ""internalType"": ""address"",
                    ""name"": ""player"",
                    ""type"": ""address""
                },
                {
                    ""indexed"": false,
                    ""internalType"": ""uint256"",
                    ""name"": ""score"",
                    ""type"": ""uint256""
                },
                {
                    ""indexed"": false,
                    ""internalType"": ""uint256"",
                    ""name"": ""timestamp"",
                    ""type"": ""uint256""
                },
                {
                    ""indexed"": false,
                    ""internalType"": ""uint256"",
                    ""name"": ""level"",
                    ""type"": ""uint256""
                },
                {
                    ""indexed"": false,
                    ""internalType"": ""string"",
                    ""name"": ""fileHash"",
                    ""type"": ""string""
                }
            ],
            ""name"": ""ScoreSaved"",
            ""type"": ""event""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": """",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""scores"",
            ""outputs"": [
                {
                    ""internalType"": ""address"",
                    ""name"": ""player"",
                    ""type"": ""address""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""score"",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""timestamp"",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""level"",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""string"",
                    ""name"": ""fileHash"",
                    ""type"": ""string""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function"",
            ""constant"": true
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_score"",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""_level"",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""string"",
                    ""name"": ""_fileHash"",
                    ""type"": ""string""
                }
            ],
            ""name"": ""saveScore"",
            ""outputs"": [],
            ""stateMutability"": ""nonpayable"",
            ""type"": ""function""
        },
        {
            ""inputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": ""index"",
                    ""type"": ""uint256""
                }
            ],
            ""name"": ""getScore"",
            ""outputs"": [
                {
                    ""internalType"": ""address"",
                    ""name"": """",
                    ""type"": ""address""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": """",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": """",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""uint256"",
                    ""name"": """",
                    ""type"": ""uint256""
                },
                {
                    ""internalType"": ""string"",
                    ""name"": """",
                    ""type"": ""string""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function"",
            ""constant"": true
        },
        {
            ""inputs"": [],
            ""name"": ""getScoreCount"",
            ""outputs"": [
                {
                    ""internalType"": ""uint256"",
                    ""name"": """",
                    ""type"": ""uint256""
                }
            ],
            ""stateMutability"": ""view"",
            ""type"": ""function"",
            ""constant"": true
        }
    ]";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void SetSelectedLevel(int levelId)
    {
        selectedLevelId = levelId;
    }

    public async void LoadLevelStats()
    {
        string modeName = "";
        int levelNumber = 0;

        switch (selectedLevelId)
        {
            case 1: modeName = "Afferra"; levelNumber = 1; break;
            case 2: modeName = "Afferra"; levelNumber = 2; break;
            case 3: modeName = "Colpisci"; levelNumber = 1; break;
            case 4: modeName = "Colpisci"; levelNumber = 2; break;
        }

        textLevelStats.text = $"{modeName}  :  Livello {levelNumber}";

        var scores = await GetScoresFromBlockchain();

        var allUsers = new List<(string Username, string Address)>();
        foreach (var kvp in UserAccounts.accountMap)
        {
            allUsers.Add((kvp.Key, kvp.Value.address));
        }
        
        var ordered = new List<UserScoreInfo>();

        foreach (var user in allUsers)
        {
            var userScore = scores
                .Where(s => s.PlayerAddress.Equals(user.Address, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(s => s.Score)
                .ThenBy(s => s.Timestamp)
                .FirstOrDefault();

            if (userScore != null)
            {
                ordered.Add(new UserScoreInfo
                {
                    Username = user.Username,
                    Score = userScore.Score,
                    Timestamp = userScore.Timestamp
                });
            }
            else
            {
                ordered.Add(new UserScoreInfo
                {
                    Username = user.Username,
                    Score = -1
                });
            }
        }

        var finalOrder = ordered
            .Where(o => o.Score >= 0)
            .OrderByDescending(o => o.Score)
            .ThenBy(o => o.Timestamp)
            .Concat(
                ordered.Where(o => o.Score < 0)
                       .OrderBy(o => o.Username)
            )
            .ToList();

        for (int i = 0; i < usersButtons.Length; i++)
        {
            var textComponent = usersButtons[i].GetComponentInChildren<TMP_Text>();

            if (i < finalOrder.Count)
            {
                var entry = finalOrder[i];
                if (entry.Score >= 0)
                {
                    DateTime italianDate = DateTimeOffset.FromUnixTimeSeconds(entry.Timestamp).ToLocalTime().DateTime;

                    textComponent.text = $"{entry.Username}  |  Record: {entry.Score}  |  {italianDate:dd/MM/yy HH:mm:ss}";
                }
                else
                {
                    textComponent.text = $"{entry.Username}  |  Record: Vuoto";
                }
            }
            else
            {
                textComponent.text = "";
            }
        }
    }

    private async Task<List<ScoreEntry>> GetScoresFromBlockchain()
    {
        var web3 = new Web3(rpcUrl);
        var contract = web3.Eth.GetContract(abi, contractAddress);

        var getScoreCountFunction = contract.GetFunction("getScoreCount");
        var getScoreFunction = contract.GetFunction("getScore");

        var count = await getScoreCountFunction.CallAsync<uint>();

        var result = new List<ScoreEntry>();

        for (uint i = 0; i < count; i++)
        {
            var scoreData = await getScoreFunction.CallDeserializingToObjectAsync<ScoreEntryDTO>(i);

            if (scoreData.Level == selectedLevelId)
            {
                result.Add(new ScoreEntry
                {
                    PlayerAddress = scoreData.Player,
                    Score = (int)scoreData.Score,
                    Timestamp = (long)scoreData.Timestamp,
                    Level = (int)scoreData.Level
                });
            }
        }

        return result;
    }

    [FunctionOutput]
    public class ScoreEntryDTO
    {
        [Parameter("address", "player", 1)]
        public string Player { get; set; }

        [Parameter("uint256", "score", 2)]
        public BigInteger Score { get; set; }

        [Parameter("uint256", "timestamp", 3)]
        public BigInteger Timestamp { get; set; }

        [Parameter("uint256", "level", 4)]
        public BigInteger Level { get; set; }
    }

    public class ScoreEntry
    {
        public string PlayerAddress;
        public int Score;
        public long Timestamp;
        public int Level;
    }

    public class UserScoreInfo
    {
        public string Username;
        public int Score;
        public long Timestamp;
    }

    public void LoadUserStats(string username)
    {
        textUserStatsTitle.text = $"Statistiche {username}";

        string modeName = "";
        int levelNumber = 0;

        switch (selectedLevelId)
        {
            case 1: modeName = "Afferra"; levelNumber = 1; break;
            case 2: modeName = "Afferra"; levelNumber = 2; break;
            case 3: modeName = "Colpisci"; levelNumber = 1; break;
            case 4: modeName = "Colpisci"; levelNumber = 2; break;
        }

        textUserStatsLevel.text = $"{modeName}  :  Livello {levelNumber}";
        
        StartCoroutine(LoadUserStatsCoroutine(username));
    }
    
    private IEnumerator LoadUserStatsCoroutine(string username)
    {
        var scoresTask = GetScoresFromBlockchain();
        yield return new WaitUntil(() => scoresTask.IsCompleted);
        
        var scores = scoresTask.Result;
        
        if (!UserAccounts.accountMap.TryGetValue(username, out var account))
        {
            Debug.LogWarning($"Utente {username} non trovato.");
            foreach (var btn in statsButtons)
            {
                var textComp = btn.GetComponentInChildren<TMP_Text>();
                if (textComp != null)
                    textComp.text = "Vuoto";
            }
            yield break;
        }
        
        var userScores = scores
            .Where(s => s.PlayerAddress.Equals(account.address, StringComparison.OrdinalIgnoreCase))
            .OrderBy(s => s.Timestamp)
            .ToList();
            
        var record = userScores
            .OrderByDescending(s => s.Score)
            .ThenBy(s => s.Timestamp)
            .FirstOrDefault();
        
        var lastThree = userScores
            .OrderByDescending(s => s.Timestamp)
            .Take(3)
            .ToList();
        
        string FormatBestScore(ScoreEntry e)
        {
            if (e == null) return "Vuoto";
            DateTime italianDate = DateTimeOffset.FromUnixTimeSeconds(e.Timestamp).ToLocalTime().DateTime;
            return $"Best score: {e.Score}    |    {italianDate:dd/MM/yy HH:mm:ss}";
        }

        string FormatScore(ScoreEntry e)
        {
            if (e == null) return "Vuoto";
            DateTime italianDate = DateTimeOffset.FromUnixTimeSeconds(e.Timestamp).ToLocalTime().DateTime;
            return $"Score: {e.Score}    |    {italianDate:dd/MM/yy HH:mm:ss}";
        }
        
        statsButtons[0].GetComponentInChildren<TMP_Text>().text = record != null ? FormatBestScore(record) : "Vuoto";
        statsButtons[1].GetComponentInChildren<TMP_Text>().text = lastThree.Count > 0 ? FormatScore(lastThree[0]) : "Vuoto";
        statsButtons[2].GetComponentInChildren<TMP_Text>().text = lastThree.Count > 1 ? FormatScore(lastThree[1]) : "Vuoto";
        statsButtons[3].GetComponentInChildren<TMP_Text>().text = lastThree.Count > 2 ? FormatScore(lastThree[2]) : "Vuoto";
    }
}

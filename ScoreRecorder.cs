using System.Threading.Tasks;
using UnityEngine;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using System.Diagnostics;
using System.IO;

public class ScoreRecorder : MonoBehaviour
{
    private string contractAddress = "0x60C92C0E63E4A2070dd96ACCd7DD1e906484CaE0";
    private string rpcUrl = "http://192.168.1.102:7545";
    private string timingFileName = "BlockchainWritingTime.csv";

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

    public async Task RecordScoreAsync(int score, int level, string filePath)
    {
        var privateKey = UserManager.Instance.PrivateKey;
        var address = UserManager.Instance.Address;

        var account = new Nethereum.Web3.Accounts.Account(privateKey);
        var web3 = new Web3(account, rpcUrl);

        var contract = web3.Eth.GetContract(abi, contractAddress);
        var saveScoreFunction = contract.GetFunction("saveScore");
        var gas = new HexBigInteger(300000);

        string fileHash = ComputeFileHash(filePath);

        var stopwatch = Stopwatch.StartNew();

        string txHash = await saveScoreFunction.SendTransactionAsync(address, gas, null, score, level, fileHash);
        var receipt = await web3.TransactionReceiptPolling.PollForReceiptAsync(txHash);

        stopwatch.Stop();

        long elapsedMs = stopwatch.ElapsedMilliseconds;
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string timingFilePath = Path.Combine(Application.persistentDataPath, timingFileName);
        string line = $"{txHash},{timestamp},{elapsedMs}";

        if (!File.Exists(timingFilePath))
        {
            File.WriteAllText(timingFilePath, "txHash,timestamp,tempoScrittura(Ms)\n" + line + "\n");
        }
        else
        {
            File.AppendAllText(timingFilePath, line + "\n");
        }
        
        UnityEngine.Debug.Log($"Livello: {level}, Score: {score}, Utente: {UserManager.Instance.SelectedUsername} - registrato sulla blockchain");
    }

    public string CreateHandsRecorderCsv(int level, string userAddress)
    {
        string shortAddress = userAddress.Substring(0, 8);
        string filename = $"HandsMovements_Level{level}_{shortAddress}_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv";
        string directoryPath = Path.Combine(Application.persistentDataPath, "HandsMovementsRecorded");
        
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = Path.Combine(directoryPath, filename);
        return filePath;
    }

    private string ComputeFileHash(string filePath)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        using (var stream = File.OpenRead(filePath))
        {
            var hashBytes = sha256.ComputeHash(stream);
            return System.BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
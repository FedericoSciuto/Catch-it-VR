using UnityEngine;
using System.Collections;
using TMPro;

public class BallLauncherLvl1 : MonoBehaviour
{
    public GameObject ballPrototype;
    public Transform spawnPoint;
    public AudioSource audioSource;
    public AudioClip printerSound;
    public AudioClip launchSound;
    public BallCounterUI ballCounterUI;
    public GameObject resultsMenu;
    public TextMeshProUGUI textScore;
    public int totalBalls = 0;
    public int level = 0;

    private float launchForce = 6.75f;
    private bool sequenceStarted = false;
    private HandsRecorder handsRecorder;
    private ScoreRecorder scoreRecorder;
    private string lastFilePath;

    void Start()
    {
        ballCounterUI.SetTotalBalls(totalBalls);
        StartCoroutine(StartAfterDelay(3f));

        handsRecorder = FindFirstObjectByType<HandsRecorder>();
        scoreRecorder = FindFirstObjectByType<ScoreRecorder>();
    }

    IEnumerator StartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(LaunchSequence());
    }

    IEnumerator LaunchSequence()
    {
        if (sequenceStarted) yield break;
        sequenceStarted = true;
        
        audioSource.PlayOneShot(printerSound);
        yield return new WaitForSeconds(printerSound.length);

        bool hasUser = UserManager.Instance.HasUser();

        if (hasUser)
        {
            lastFilePath = scoreRecorder.CreateHandsRecorderCsv(level, UserManager.Instance.Address);
            handsRecorder.StartRecording(lastFilePath);
        }

        for (int i = 0; i < totalBalls; i++)
        {
            if (hasUser)
            {
                handsRecorder.StartRecordingSegment();
            }

            LaunchBall();
            yield return new WaitForSeconds(hasUser ? 2f : 3f);
            
            if (hasUser)
            {
                handsRecorder.StopRecordingSegment();
                yield return new WaitForSeconds(1f);
            }
        }

        if (hasUser)
        {
            handsRecorder.SaveBufferToFile();
        }
        
        ShowResults();
    }

    void LaunchBall()
    {
        GameObject ball = Instantiate(ballPrototype, spawnPoint.position, Quaternion.identity);
        ball.SetActive(true);
        
        audioSource.PlayOneShot(launchSound);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(spawnPoint.forward * launchForce, ForceMode.Impulse);
        
        ballCounterUI.BallLaunched();
    }

    void ShowResults()
    {
        Time.timeScale = 0f;
        resultsMenu.SetActive(true);

        string scoreText = ballCounterUI.GetScore();
        textScore.text = scoreText;

        string[] parts = scoreText.Split(':', '/');
        if (parts.Length >= 2 && int.TryParse(parts[1].Trim(), out int score))
        {
            if (!string.IsNullOrEmpty(lastFilePath))
            {
                _ = scoreRecorder.RecordScoreAsync(score, level, lastFilePath);
            }
            else
            {
                Debug.Log("Nessun utente selezionato, punteggio non registrato");
            }
        }
    }
}

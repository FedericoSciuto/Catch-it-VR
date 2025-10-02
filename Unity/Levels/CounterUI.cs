using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BallCounterUI : MonoBehaviour
{
    public TextMeshProUGUI ballCounter;
    public TextMeshProUGUI grabCounter;

    private int totalBalls = 0;
    private int ballsLaunched = 0;
    private int ballsCaught = 0;

    private HashSet<GameObject> countedBalls = new HashSet<GameObject>();

    public void SetTotalBalls(int total)
    {
        totalBalls = total;
        UpdateRemainingText();
    }

    public void BallLaunched()
    {
        ballsLaunched++;
        UpdateRemainingText();
    }

    public void BallCaught(GameObject ball)
    {
        if (countedBalls.Contains(ball))
            return;

        countedBalls.Add(ball);
        ballsCaught++;
        UpdateCaughtText();
    }

    private void UpdateRemainingText()
    {
        int remaining = totalBalls - ballsLaunched;
        ballCounter.text = $"{remaining}  |  {totalBalls}";
    }

    private void UpdateCaughtText()
    {
        grabCounter.text = $"{ballsCaught}";
    }

    public string GetScore()
    {
        return $"Score: {ballsCaught}/{totalBalls}";
    }
}

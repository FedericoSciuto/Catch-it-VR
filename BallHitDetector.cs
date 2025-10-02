using UnityEngine;

public class BallTouchDetector : MonoBehaviour
{
    public BallCounterUI ballCounterUI;

    private bool hasBeenTouched = false;

    void OnCollisionEnter(Collision collision)
    {
        if (hasBeenTouched)
            return;

        if (collision.gameObject.CompareTag("Hand"))
        {
            hasBeenTouched = true;
            if (ballCounterUI != null)
            {
                ballCounterUI.BallCaught(gameObject);
            }
        }
    }
}
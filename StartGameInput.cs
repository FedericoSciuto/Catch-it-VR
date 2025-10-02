using UnityEngine;
using UnityEngine.Playables;

public class StartGameInput : MonoBehaviour
{
    private bool canStart = false;
    public PlayableDirector startMainMenuDirector;

    public void EnableStart()
    {
        canStart = true;
    }

    void Update()
    {
        if (!canStart) return;

        if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
        {
            startMainMenuDirector.Play();
        }
    }
}
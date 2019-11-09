using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Input;

public class InputManager : MonoBehaviour
{
    public ClickableObject[] openAirClickHandlers;
    public int activeOpenAirClickIndex = 0;

    public static InputManager singleton { get; private set; }
    public bool readyGesture { get; private set; }

    private GestureRecognizer m_gestureRecognizer;
    private AudioSource m_audioSource;

    // Use this for initialization
    void Start()
    {
        if (singleton != null)
        {
            throw new System.Exception("Cannot have more than one InputManager");
        }
        singleton = this;

        m_audioSource = gameObject.GetComponent<AudioSource>();

        m_gestureRecognizer = new GestureRecognizer();
        m_gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

        // Source detected/lost used to show cursor only when ready gesture is invoked
        InteractionManager.SourceDetected += InteractionManager_SourceDetected;
        InteractionManager.SourceLost += InteractionManager_SourceLost;

        // Tapped gesture used for basic 'click' functionality
        m_gestureRecognizer.TappedEvent += m_gestureRecognizer_TappedEvent;

        m_gestureRecognizer.StartCapturingGestures();
    }

    private void InteractionManager_SourceLost(InteractionSourceState state)
    {
        Debug.Log(string.Format("SourceLost"));
        readyGesture = false;
    }

    private void InteractionManager_SourceDetected(InteractionSourceState state)
    {
        Debug.Log(string.Format("SourceDetected"));
        readyGesture = true;
    }

    private void m_gestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        Debug.Log(string.Format("TappedEvent"));
        m_audioSource.PlayOneShot(m_audioSource.clip);

        GameObject targetGameObject = RaycastForInput.singleton.GetGameObjectUnderCursor();

        if (targetGameObject != null)
        {
            ClickableObject clickTarget = targetGameObject.GetComponent<ClickableObject>();
            if (clickTarget != null)
            {
                if (targetGameObject.layer == 31)
                {
                    openAirClickHandlers[activeOpenAirClickIndex].OnClick();
                }
                else
                {
                    clickTarget.OnClick();
                }
            }
            else
            {
                if (targetGameObject.layer == 31)
                {
                    openAirClickHandlers[activeOpenAirClickIndex].OnClick();
                }
            }
        }
        else
        {
            if (openAirClickHandlers != null)
            {
                if (activeOpenAirClickIndex < openAirClickHandlers.Length)
                {
                    openAirClickHandlers[activeOpenAirClickIndex].OnClick();
                }
            }
        }
    }
}

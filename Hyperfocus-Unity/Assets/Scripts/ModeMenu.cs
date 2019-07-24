using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeMenu : MonoBehaviour
{
    private Transform m_transform;

    private bool m_prevReadyState;
    private bool m_currentReadyState;

    // Use this for initialization
    void Start()
    {
        m_transform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        m_currentReadyState = InputManager.singleton.readyGesture;

        // Transition to/from ready state
        if(m_currentReadyState != m_prevReadyState)
        {
            m_transform.position = HolographicCursor.singleton.cursorTransform.position;
            m_transform.LookAt(Camera.main.transform.position, Vector3.up);
            InputManager.singleton.activeOpenAirClickIndex = 2;
        }

        m_prevReadyState = m_currentReadyState;
    }
}

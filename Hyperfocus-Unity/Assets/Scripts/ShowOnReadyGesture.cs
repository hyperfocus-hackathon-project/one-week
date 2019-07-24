using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnReadyGesture : MonoBehaviour
{
    private MeshRenderer m_meshRenderer;
    private Collider m_collider;

    private MeshRenderer[] m_meshRenderers;
    private Collider[] m_colliders;
    private void Start()
    {
        m_meshRenderer = gameObject.GetComponent<MeshRenderer>();
        m_collider = gameObject.GetComponent<Collider>();

        m_meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        m_colliders = gameObject.GetComponentsInChildren<Collider>();
    }
    void Update()
    {

        for (int i = 0; i < m_meshRenderers.Length; i++)
        {
            m_meshRenderers[i].enabled = InputManager.singleton.readyGesture;
        }

        for (int i = 0; i < m_colliders.Length; i++)
        {
            m_colliders[i].enabled = InputManager.singleton.readyGesture;
        }
        //if(m_meshRenderer != null) { m_meshRenderer.enabled = InputManager.singleton.readyGesture; }
        //if(m_collider != null) { m_collider.enabled = InputManager.singleton.readyGesture; }
    }
}

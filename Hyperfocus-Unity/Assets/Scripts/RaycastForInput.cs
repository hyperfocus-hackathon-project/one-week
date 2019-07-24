using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastForInput : MonoBehaviour
{
    public static RaycastForInput singleton { get; private set; }

    private Transform m_cameraTransform;
    private Transform m_cursorTransform;

    private Ray m_ray;
    private RaycastHit m_hit;

    public RaycastHit raycastHit { get { return m_hit; } }

    void Start()
    {
        if (singleton != null)
        {
            throw new System.Exception("Cannot have more than one RaycastForInput");
        }
        singleton = this;

        m_cameraTransform = Camera.main.transform;
        m_cursorTransform = HolographicCursor.singleton.cursorTransform;
    }

    public GameObject GetGameObjectUnderCursor()
    {
        GameObject result = null;

        m_ray = new Ray(m_cameraTransform.position, m_cameraTransform.forward);
        if (Physics.Raycast(m_ray, out m_hit, 8))
        {
            result = m_hit.collider.gameObject;
        }

        return result;
    }

    public float GetDistanceToGameObjectUnderCursor()
    {
        float distance = -1;

        GameObject result = GetGameObjectUnderCursor();
        if (result != null)
        {
            distance = Vector3.Distance(m_cameraTransform.position, result.transform.position);
        }

        return distance;
    }

    public Vector3 GetRaycastNormal()
    {
        return m_hit.normal;
    }

    public float GetDistanceToCursor()
    {
        return Vector3.Distance(m_cameraTransform.position, m_cursorTransform.position);
    }

    public Vector3 GetCursorPosition()
    {
        return m_cursorTransform.position;
    }
}

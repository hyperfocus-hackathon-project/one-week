using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolographicCursor : MonoBehaviour
{
    public static HolographicCursor singleton { get; private set; }
    public Transform cursorTransform
    {
        get { return cursorInstance.transform; }
    }

    public GameObject cursorPrefab;
    public float maxCursorDistance = 4.0f;

    private float distanceToObject;
    private GameObject cursorInstance;
    private Transform m_cameraTransform;

    // Use this for initialization
    void Start()
    {
        if (singleton != null)
        {
            throw new System.Exception("Cannot have more than one HolographicCursor");
        }
        singleton = this;

        m_cameraTransform = Camera.main.transform;

        cursorInstance = GameObject.Instantiate(cursorPrefab);
        cursorInstance.transform.parent = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        cursorInstance.SetActive(InputManager.singleton.readyGesture);
        if (InputManager.singleton.readyGesture)
        {
            distanceToObject = RaycastForInput.singleton.GetDistanceToGameObjectUnderCursor();

            if (distanceToObject > 0)
            {
                cursorInstance.transform.forward = RaycastForInput.singleton.GetRaycastNormal();
                cursorInstance.transform.position = RaycastForInput.singleton.raycastHit.point + (cursorInstance.transform.forward * 0.02f);
            }
            else
            {
                cursorInstance.transform.position = m_cameraTransform.position + (m_cameraTransform.forward * maxCursorDistance);
                cursorInstance.transform.LookAt(m_cameraTransform.position, Vector3.up);
            }
        }
    }
}

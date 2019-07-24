using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float speed = 1.0f;
    public float minSize = 0.8f;
    public float maxSize = 1.2f;

    private float m_currentSize;
    private Transform m_transform;

    // Use this for initialization
    void Start()
    {
        m_transform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float f = Mathf.Abs(Mathf.Sin(Time.time));
        m_currentSize = Mathf.Lerp(minSize, maxSize, f);
        m_transform.localScale = Vector3.one * m_currentSize;
    }
}

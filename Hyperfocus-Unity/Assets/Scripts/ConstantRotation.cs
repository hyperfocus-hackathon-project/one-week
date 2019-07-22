using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public Vector3 direction = Vector3.zero;
    public float speed = 1.0f;

    private Transform m_transform;

    void Start()
    {
        m_transform = gameObject.transform;
    }

    void Update()
    {
        m_transform.Rotate(direction * speed);
    }
}

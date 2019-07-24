using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimeout : MonoBehaviour
{
    public float timeoutInSeconds = 10.0f;

    private float m_elapsedTime;

    // Update is called once per frame
    void Update()
    {
        m_elapsedTime += Time.smoothDeltaTime;
        if(m_elapsedTime >= timeoutInSeconds)
        {
            GameObject.Destroy(gameObject);
        }
    }
}

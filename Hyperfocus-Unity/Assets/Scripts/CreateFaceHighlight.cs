using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFaceHighlight : ClickableObject
{
    public GameObject highlightPrefab;
    private GameObject m_highlightInstance;

    public override void OnClick()
    {
        AddHighlight();
        base.OnClick();
    }

    private void AddHighlight()
    {
        m_highlightInstance = GameObject.Instantiate(highlightPrefab);
        m_highlightInstance.transform.position = HolographicCursor.singleton.cursorTransform.position;
        m_highlightInstance.transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}

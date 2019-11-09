using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSurfaceShade : ClickableObject
{
    public GameObject shadePrefab;
    private GameObject m_shadeInstance;

    public GameObject closeButtonPrefab;
    private GameObject m_closeButtonInstance;

    private Vector3 m_firstPoint;
    private Vector3 m_secondPoint;

    private enum CreateShadeStateEnum { FirstPoint, SecondPoint }
    private CreateShadeStateEnum m_state;


    public override void OnClick()
    {
        switch (m_state)
        {
            case CreateShadeStateEnum.FirstPoint:
                m_firstPoint = HolographicCursor.singleton.cursorTransform.position;
                m_state = CreateShadeStateEnum.SecondPoint;
                break;
            case CreateShadeStateEnum.SecondPoint:
                m_secondPoint = HolographicCursor.singleton.cursorTransform.position;
                CreateShade();
                m_state = CreateShadeStateEnum.FirstPoint;
                break;
            default:
                break;
        }
        base.OnClick();
    }

    private void CreateShade()
    {
        m_shadeInstance = GameObject.Instantiate(shadePrefab);

        Vector3 centerPosition = (m_firstPoint + m_secondPoint) / 2;

        // Diagonal distance
        float cSqr = Vector3.Distance(m_firstPoint, m_secondPoint);
        cSqr *= cSqr;
        // Vertical length (height)
        float bSqr = Vector3.Distance(new Vector3(0, m_firstPoint.y, 0), new Vector3(0, m_secondPoint.y, 0));
        float height = bSqr;
        bSqr *= bSqr;

        // Horizontal length (width)
        float aSqr = cSqr - bSqr;
        float width = Mathf.Sqrt(aSqr);

        m_shadeInstance.transform.position = centerPosition;
        m_shadeInstance.transform.localScale = new Vector3(width, height, 0.2f);
        m_shadeInstance.transform.LookAt(Camera.main.transform.position, Vector3.up);

        m_closeButtonInstance = GameObject.Instantiate(closeButtonPrefab);
        m_closeButtonInstance.transform.position = centerPosition + (m_shadeInstance.transform.forward * 0.5f);
        m_closeButtonInstance.transform.eulerAngles = m_shadeInstance.transform.eulerAngles;
        m_closeButtonInstance.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
        m_closeButtonInstance.GetComponent<DestroyOnClick>().destroyTarget = m_shadeInstance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnClick : ClickableObject
{
    public GameObject destroyTarget;
    public bool destroySelf = false;

    public override void OnClick()
    {
        GameObject.DestroyImmediate(destroyTarget);
        if(destroySelf == true)
        {
            GameObject.Destroy(gameObject);
        }
    }
}

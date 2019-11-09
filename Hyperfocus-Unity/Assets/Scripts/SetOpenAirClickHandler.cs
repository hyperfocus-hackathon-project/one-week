using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOpenAirClickHandler : ClickableObject
{
    public int modeIndex = 0;

    public override void OnClick()
    {
        InputManager.singleton.activeOpenAirClickIndex = modeIndex;
        base.OnClick();
    }
}

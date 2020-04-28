using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerBtn : Button
{
    public bool isPressed = false;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (eventData.button == PointerEventData.InputButton.Left)
            isPressed = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (eventData.button == PointerEventData.InputButton.Left)
            isPressed = false;
    }
}

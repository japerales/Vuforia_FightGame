using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{

    [HideInInspector]
    public bool Pressed;
    [HideInInspector]
    public bool JustPressed;

 

    public void OnPointerDown(PointerEventData data)
    {
        Pressed = true;

       
        JustPressed = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        Pressed = false;
    }

    public void Update()
    {
        if (JustPressed)
            JustPressed = false;
    }

}

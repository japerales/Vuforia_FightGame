using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputAndroid : Singleton<InputAndroid>
{
    public List<ButtonPointerHandler> buttonList;

    private ButtonPointerHandler AxisXNegative;
    private ButtonPointerHandler AxisXPositive;

    //private ButtonPointerHandler AxisXN;
    //private ButtonPointerHandler AxisXP;
    public float SmoothFactor = 2f;
    private float currentXAxisValue;
    // Use this for initialization
    void Start()
    {
        //mapeamos los botones para no tener que buscarlos
        AxisXNegative = buttonList[0].GetComponent<ButtonPointerHandler>();
        AxisXPositive = buttonList[1].GetComponent<ButtonPointerHandler>();

        currentXAxisValue = 0f;
    }

    public float GetXAxisRaw()
    {
        return GetMapAxisValue();
    }

    public float GetXAxis()
    {
        currentXAxisValue = Mathf.MoveTowards(currentXAxisValue, GetMapAxisValue(), Time.deltaTime * SmoothFactor);
        return currentXAxisValue;
    }

    private float GetMapAxisValue()
    {
        float direction = 0f;
        if (AxisXNegative.Pressed)
            direction = -1f;
        if (AxisXPositive.Pressed)
            direction = 1f;
        return direction;
    }

    public bool GetButtonDown(string name)
    {
        foreach (ButtonPointerHandler b in buttonList)
        {
            if (b.gameObject.name.Equals(name))
                return b.Pressed;
        }
        return false;
    }

    public bool GetButtonPressed(string name)
    {
        foreach (ButtonPointerHandler b in buttonList)
        {
            if (b.gameObject.name.Equals(name))
                return b.JustPressed;
        }
        return false;
    }

}

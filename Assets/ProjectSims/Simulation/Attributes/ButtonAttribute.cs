using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class ButtonAttribute : PropertyAttribute
{
    public string ButtonText;

    public ButtonAttribute(string buttonText = null)
    {
        ButtonText = buttonText;
    }
}
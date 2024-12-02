using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lamp : MonoBehaviour
{
    [SerializeField] private Light2D _light;

    private void Start()
    {
        if (_light == null)
        {
            _light = GetComponentInChildren<Light2D>();
        }
    }

    public void TurnOn()
    {
        _light.intensity = 1;
    }

    public void TurnOff()
    {
        _light.intensity = 0;
    }
}

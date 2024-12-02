using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour, IEnvironmentUpdater
{
    private float _t;
    public Gradient _gradient;
    public Light2D _light;
    public float _intensityMultiplier;

    public void UpdateControl(float _progress)
    {
        _light.color = _gradient.Evaluate(_progress);
        _light.intensity = _light.color.a * _intensityMultiplier;
    }
}

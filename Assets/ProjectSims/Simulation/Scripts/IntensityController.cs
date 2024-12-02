using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IntensityController : MonoBehaviour, IEnvironmentUpdater
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private Light2D _light;
    
    public void UpdateControl(float progress)
    {
        var value = _curve.Evaluate(progress);
        _light.intensity = value;
    }
}

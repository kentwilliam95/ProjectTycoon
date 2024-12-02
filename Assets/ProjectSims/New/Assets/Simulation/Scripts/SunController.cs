using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour, IEnvironmentUpdater
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _degree;
    
    public void UpdateControl(float progress)
    {
        var rotation = _curve.Evaluate(progress) * _degree;
        transform.rotation = Quaternion.Euler(0,0,rotation);
    }
}

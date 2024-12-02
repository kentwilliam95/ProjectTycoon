using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StarController : MonoBehaviour, IEnvironmentUpdater
{
    [SerializeField] private Star[] _stars;

    private void Start()
    {
        _stars = GetComponentsInChildren<Star>(true);
    }

    public void UpdateControl(float progress)
    {
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].SetIntensity(progress);
        }
    }
}
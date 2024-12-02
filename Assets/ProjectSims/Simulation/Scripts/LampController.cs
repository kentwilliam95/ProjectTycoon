using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampController : MonoBehaviour, IEnvironmentUpdater
{
    private Lamp[] _lamps;
    private void Start()
    {
        _lamps = GetComponentsInChildren<Lamp>();
    }

    public void UpdateControl(float progress)
    {
        if (progress is > 0 and < 0.25f)
        {
            for (int i = 0; i < _lamps.Length; i++)
            {
                _lamps[i].TurnOn();
            }   
        }
        else if (progress> 0.75f)
        {
            for (int i = 0; i < _lamps.Length; i++)
            {
                _lamps[i].TurnOn();
            }   
        }
        else
        {
            for (int i = 0; i < _lamps.Length; i++)
            {
                _lamps[i].TurnOff();
            }   
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private IEnvironmentUpdater[] IUpdater;
    private TimeModule _timeModule;

    [SerializeField] private TMP_Text _text;
    public int _speed = 1;

    private void Start()
    {
        IUpdater = GetComponentsInChildren<IEnvironmentUpdater>();
        _timeModule = new TimeModule(0);
        _timeModule.Speed = _speed;
    }

    private void Update()
    {
        var dt = Time.deltaTime;
        _timeModule.Update(dt);
        _text.SetText(_timeModule.ToString());
        
        var progress = _timeModule.DayProgression;
        for (int i = 0; i < IUpdater.Length; i++)
        {
            IUpdater[i].UpdateControl(progress);
        }
    }

    public void SetSpeed(int speed)
    {
        _timeModule.Speed = speed;
    }
}

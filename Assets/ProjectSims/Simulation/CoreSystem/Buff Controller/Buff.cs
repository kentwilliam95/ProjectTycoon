using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.BuffSystem
{
    public class Buff
    {
        private float _speed = 1;
        private float _counter;

        private float _tickCount;
        private float _duration;
        private float _tick;
        private bool _isComplete;

        private float _dmg;

        private StatusController.Stats _affected;
        private StatusController _statusController;
        private System.Action<Buff> _onComplete;

        public Buff(StatusController controller ,StatusController.Stats affected, float dmg, float tick, float totalTime,
            System.Action<Buff> onComplete)
        {
            _onComplete = onComplete;
            _affected = affected;
            _duration = totalTime;
            _isComplete = false;
            _tick = tick;
            _statusController = controller;
            _dmg = dmg;
        }

        public void Update(float dt)
        {
            if (_isComplete)
            {
                return;
            }

            _tickCount -= Time.deltaTime;
            if (_tickCount <= 0)
            {
                _statusController.Add(_affected, _dmg);
                Debug.Log($"{_affected.ToString()} : { _statusController.GetStat(_affected)}");
                _tickCount = _tick;
            }

            _counter += dt;
            if (_counter >= _duration)
            {
                _isComplete = true;
                _onComplete?.Invoke(this);
            }
        }
    }
}
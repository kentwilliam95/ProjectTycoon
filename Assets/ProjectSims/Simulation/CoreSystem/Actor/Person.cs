using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation;
using Simulation.BuffSystem;

namespace Simulation
{
    public class Person
    {
        private Activity _activity;
        private StatusController _statusController;
        public StatusController StatusController => _statusController;

        private List<Buff> _listBuff;
        
        public Person()
        {
            _statusController = new StatusController();
            _listBuff = new List<Buff>();
            _activity = new ActivityWaiting(this);
        }

        public void ApplyBuff(Buff buff)
        {
            _listBuff.Add(buff);
        }

        public void RemoveBuff(Buff buff)
        {
            var isContain = _listBuff.Contains(buff);
            if (!isContain) { return; }

            _listBuff.Remove(buff);
        }

        private void UpdateBuff(float dt)
        {
            if (_listBuff.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < _listBuff.Count; i++)
            {
                if (_listBuff[i] == null) { continue; }
                _listBuff[i].Update(dt);
            }
        }

        public void Update(float dt)
        {
            UpdateStatus(dt);
            UpdateBuff(dt);
        }

        private void UpdateStatus(float dt) { }

        public void ChangeActivity(Activity activity) { }
    }
}
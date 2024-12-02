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

        private BuffController _buffCtrl;
        
        public Person()
        {
            _statusController = new StatusController();
            _buffCtrl = new BuffController();
            _activity = new ActivityWaiting(this);
        }

        public void ApplyBuff(Buff buff)
        {
            _buffCtrl.Add(buff);
        }

        public void RemoveBuff(Buff buff)
        {
            _buffCtrl.Remove(buff);
        }

        public void Update(float dt)
        {
            UpdateStatus(dt);
            _buffCtrl.Update(dt);
        }

        private void UpdateStatus(float dt) { }

        public void ChangeActivity(Activity activity) { }
    }
}
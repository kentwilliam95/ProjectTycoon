using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation;
using Simulation.BuffSystem;

namespace Simulation
{
    public enum ActivityType
    {
        Walking,
        SearchADrink,
        Drinking
    }
    
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
            // _activity = new ActivityWalking(this);
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
            
            UpdateActivity(dt);

            if (Input.GetKeyDown(KeyCode.S))
            {
                CoreController.Instance.Stall.CustomerVisitStall(this, (stall) =>
                {
                    var p = stall.Products[0];
                    stall.BuyMenu(p, (s,p) =>
                    {
                        Debug.Log($"Thank you! product:{p.Name}");
                        s.CustomerLeave(this);
                    }, (s,p) =>
                    {
                        Debug.Log($"Dissapointed!");
                        s.CustomerLeave(this);
                    });
                });
            }
        }

        private void UpdateStatus(float dt) { }

        private void UpdateActivity(float dt)
        {
            if (_activity == null) { return; }
            _activity.DoActivity(dt);
        }

        public void ChangeActivity(ActivityType activityType)
        {
            Activity next = null;
            switch (activityType)
            {
                case ActivityType.SearchADrink:
                    next = new ActivitySearchForADrink(this);
                    break;
            }

            if (_activity != null)
            {
                _activity.Exit();   
            }

            _activity = next;
        }

        public void StartWalkingTo(System.Action onComplete)
        {
            CoreController.Instance.StartCoroutine(WalkTo_Ienumerator(onComplete));
        }

        private IEnumerator WalkTo_Ienumerator(System.Action onComplete)
        {
            //wait for AI to finish the work
            yield return null;
            onComplete?.Invoke();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation;
using Simulation.BuffSystem;
using UnityEngine.AI;

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
        public const float WALKSPEED = 1.5f;
        public const float RUNSPEED = 2f;
        
        private Activity _activity;
        private StatusController _statusController;
        public StatusController StatusController => _statusController;

        private BuffController _buffCtrl;
        public PersonView View { get; private set; }
        public NavMeshAgent Agent => View.Agent;

        public Person(PersonView view)
        {
            _statusController = new StatusController();
            _buffCtrl = new BuffController();

            View = view;
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
                
                case ActivityType.Walking:
                    next = new ActivityWalking(this);
                    break;
            }

            if (_activity != null)
            {
                _activity.Exit();   
            }

            _activity = next;
        }
        
        private Coroutine _coroutineWalk;
        private NavMeshPath path = new NavMeshPath();
        public void StartMoveTo(Vector3 destination, float speed, System.Action onComplete)
        {
            StopWalking();
            NavMesh.CalculatePath(View.transform.position, destination, NavMesh.AllAreas, path);
            Agent.SetPath(path);
            Agent.speed = speed;
            
            if (_coroutineWalk != null)
            {
                CoreController.Instance.StopCoroutine(_coroutineWalk);
            }

            _coroutineWalk =  CoreController.Instance.StartCoroutine(WalkTo_IEnumerator(onComplete));
        }

        public void StopWalking()
        {
            if (_coroutineWalk != null)
            {
                CoreController.Instance.StopCoroutine(_coroutineWalk);
            }
        }

        private IEnumerator WalkTo_IEnumerator(System.Action onComplete)
        {
            //wait for AI to finish the work
            yield return null;
            while (Agent.remainingDistance > 0.1f)
            {
                yield return null;
            }
            onComplete?.Invoke();
        }
    }
}
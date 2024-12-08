using System.Collections;
using System.Collections.Generic;
using ProjectSims.Simulation.CoreSystem;
using Simulation.BuffSystem;
using UnityEngine;

namespace Simulation.BuffSystem
{
    public class ActivityWalking : Activity
    {
        private const float WALKSPEED = 1.5f;
        public const float RUNSPEED = 2f;
        
        private Person _person;
        private Buff buffThirsty;

        public ActivityWalking(Person person) : base()
        {
            _person = person;
            var target = GroundArea.Instance.GetRandomPoint();
            StartWalkingTo(target, WalkOnComplete);
            // buffThirsty = new Buff(person.StatusController, StatusController.Stats.Thirsty,-1 , 1f, 99, Buff_OnComplete);
            // person.ApplyBuff(buffThirsty);
        }

        private void WalkOnComplete()
        {
            StartWalkingTo(GroundArea.Instance.GetRandomPoint(), WalkOnComplete);
        }

        public override void DoActivity(float dt)
        {
            // var value = _person.StatusController.GetStat(StatusController.Stats.Thirsty);
            // if (value < 75)
            // {
            //     _person.ChangeActivity(ActivityType.SearchADrink);
            // }
        }

        public override void Exit()
        {
            _person.RemoveBuff(buffThirsty);
            StopWalking();
        }

        private void Buff_OnComplete(Buff buff)
        {
            _person.RemoveBuff(buff);
        }
        
        private Coroutine _coroutineWalk;
        public void StartWalkingTo(Vector3 destination, System.Action onComplete)
        {
            _person.View.Agent.SetDestination(destination);
            _person.Agent.speed = WALKSPEED;
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
            while (_person.Agent.remainingDistance > 0.1f)
            {
                yield return null;
            }
            onComplete?.Invoke();
        }
    }
}
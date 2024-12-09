using System.Collections;
using System.Collections.Generic;
using ProjectSims.Simulation.CoreSystem;
using Simulation.BuffSystem;
using UnityEngine;

namespace Simulation.BuffSystem
{
    public class ActivityWalking : Activity
    {
        private Person _person;
        private Buff buffThirsty;

        public ActivityWalking(Person person) : base()
        {
            _person = person;
            var target = GroundArea.Instance.GetRandomPoint();
            _person.StartWalkingTo(target, WalkOnComplete);
            buffThirsty = new Buff(person.StatusController, StatusController.Stats.Thirsty,-1 , 0.25f, 99, Buff_OnComplete);
            person.ApplyBuff(buffThirsty);
        }

        private void WalkOnComplete()
        {
            _person.StartWalkingTo(GroundArea.Instance.GetRandomPoint(), WalkOnComplete);
        }

        public override void DoActivity(float dt)
        {
            var value = _person.StatusController.GetStat(StatusController.Stats.Thirsty);
            if (value < 75)
            {
                _person.ChangeActivity(ActivityType.SearchADrink);
            }
        }

        public override void Exit()
        {
            _person.RemoveBuff(buffThirsty);
            _person.StopWalking();
        }

        private void Buff_OnComplete(Buff buff)
        {
            _person.RemoveBuff(buff);
        }
    }
}
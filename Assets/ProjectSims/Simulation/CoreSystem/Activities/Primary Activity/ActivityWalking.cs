using System.Collections;
using System.Collections.Generic;
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
            
            buffThirsty = new Buff(person.StatusController, StatusController.Stats.Thirsty,-1 , 0.1f, 99, Buff_OnComplete);
            person.ApplyBuff(buffThirsty);
            Debug.Log("Enter Activity Walking!");
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
        }

        private void Buff_OnComplete(Buff buff)
        {
            _person.RemoveBuff(buff);
        }
    }   
}
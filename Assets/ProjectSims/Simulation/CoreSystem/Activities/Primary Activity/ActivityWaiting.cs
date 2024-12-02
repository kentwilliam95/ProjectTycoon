using System.Collections;
using System.Collections.Generic;
using Simulation.BuffSystem;
using UnityEngine;

namespace Simulation
{
    public class ActivityWaiting : Activity
    {
        private Person _person;
        public ActivityWaiting(Person person)
        {
            Debug.Log("Enter Waiting!");
            _person = person;
            var buff = new Buff(person.StatusController, StatusController.Stats.Energy, -1f, 1f, 5f, Buff_OnComplete);
            _person.ApplyBuff(buff);
        }

        private void Buff_OnComplete(Buff buff)
        {
            _person.RemoveBuff(buff);   
        }
    }
}
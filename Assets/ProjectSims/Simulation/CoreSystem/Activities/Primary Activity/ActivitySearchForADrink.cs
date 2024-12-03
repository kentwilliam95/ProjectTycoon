using System.Collections;
using System.Collections.Generic;
using Simulation.Items;
using UnityEngine;

namespace Simulation.BuffSystem
{
    public class ActivitySearchForADrink : Activity
    {
        private enum State
        {
            None,
            Searching,
            WalkingToThatStall,
            Buying,
            Drinking,
            Finish
        }
        
        private Person _person;
        private float _duration;
        private Item _itemFromStall;

        private State _state = State.None;
        public ActivitySearchForADrink(Person person)
        {
            _person = person;
            _duration = 3f;
            _state = State.Searching;
            Debug.Log("Searching for drink stall.");
        }

        public override void DoActivity(float dt)
        {
            if (_state == State.None || _state == State.Finish) {return;}

            switch (_state)
            {
                case State.Searching:
                    HandleSearching(dt);
                    break;
                
                case State.WalkingToThatStall:
                    HandleWalkingToStall(dt);
                    break;
                
                case State.Buying:
                    HandleBuyingMenuAtStall(dt);
                    break;
                
                case State.Drinking:
                    HandleDrinking(dt);
                    break;
            }
        }

        private void HandleSearching(float dt)
        {
            _duration -= dt;
            if (_duration <= 0)
            {
                Debug.Log("Search Finish! Walking to that stall!");
                _state = State.WalkingToThatStall;
                _duration = 2f;
            }   
        }

        private void HandleWalkingToStall(float dt)
        {
            _duration -= dt;
            if (_duration <= 0)
            {
                Debug.Log("End Walking to stall");
                _state = State.Buying;
                _duration = 2f;
            }
        }

        private void HandleBuyingMenuAtStall(float dt)
        {
            _duration -= dt;
            if (_duration <= 0)
            {
                Debug.Log("End Buying!");
                _state = State.Drinking;
                _duration = 2f;

                var stall = CoreController.Instance.GetStallNearPerson(_person);
                stall.BuyMenu((item) =>
                {
                    _itemFromStall = item;
                });
                Debug.Log("Waiting for that menu to be serve!");
            }
        }

        private void HandleDrinking(float dt)
        {
            if (_itemFromStall == null) { return; }
            
            _duration -= dt;
            if (_duration <= 0)
            {
                Debug.Log("End Drinking to the next state!");
                _state = State.Finish;
            }
        }
    }   
}

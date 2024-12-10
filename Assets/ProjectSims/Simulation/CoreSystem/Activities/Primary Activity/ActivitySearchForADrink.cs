using Simulation.Products;
using Simulation.Stalls;
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
            WalkingToThatStallInProgress,
            Waiting
        }

        private Person _person;
        private float _duration;
        private EndProduct _itemFromStall;
        private Stall _stall;

        private State _state = State.None;

        public ActivitySearchForADrink(Person person)
        {
            _person = person;
            _duration = 3f;
            _state = State.Searching;
        }

        public override void DoActivity(float dt)
        {
            if (_state == State.None)
            {
                return;
            }

            switch (_state)
            {
                case State.Searching:
                    HandleSearching(dt);
                    break;

                case State.WalkingToThatStall:
                    HandleWalkingToStall(dt);
                    break;
            }
        }

        private void HandleSearching(float dt)
        {
            _duration -= dt;
            if (_duration <= 0)
            {
                _state = State.WalkingToThatStall;
                _duration = 2f;
                _stall = CoreController.Instance.GetStallNearPerson(_person);
            }
        }
        
        private void HandleWalkingToStall(float dt)
        {
            _state = State.WalkingToThatStallInProgress;
            _stall.CustomerVisitStall(_person, Handle_OnWaitingListCalled);
        }

        private void Handle_OnWaitingListCalled(Stall stall)
        {
            //Todo: add randomness to not make this npc not follow the rule
            var listProduct = stall.GetProductBaseOnStats(StatusController.Stats.Thirsty);
            var count = listProduct.Count;
            bool isGotAMenu = false;
            for (int i = 0; i < count ; i++)
            {
                var ranIndex = Random.Range(0, listProduct.Count);
                var productSO = listProduct[ranIndex];
                bool isAvailable = _stall.IsProductAvailable(productSO);
                if (isAvailable)
                {
                    isGotAMenu = true;
                    _stall.BuyMenu(productSO, Handle_StallServeMenu_OnSuccess, null);
                    _state = State.Waiting;
                    break;
                }
            }
            
            if (isGotAMenu) { return;}
            
            if (_stall.IsProductAvailable(_person.View.MinDrink))
            {
                _stall.BuyMenu(_person.View.MinDrink, Handle_StallServeMenu_OnSuccess, null);
                _state = State.Waiting;
                return;
            }

            BackToWalking();
        }

        private void Handle_StallServeMenu_OnSuccess(Stall stall, EndProduct item)
        {
            _itemFromStall = item;
            item.Use(_person);
            BackToWalking();
        }

        private void BackToWalking()
        {
            _stall.CustomerLeave(_person);
            _person.ChangeActivity(ActivityType.Walking);
        }
    }
}
using ProjectSims.Scripts.ActivityInRestaurant;
using ProjectSims.Scripts.Entities;
using ProjectSims.Scripts.Place;
using ProjectSims.Scripts.StateMachine;
using UnityEngine;

namespace ProjectSims.Scripts
{
    public class Customer : Work
    {
        public override void Initialize(Entity entity, PlaceSO place)
        {
            base.Initialize(entity, place);
            _stateMachine.ChangeState(new ActivityCustomerWaiting());
        }
    }
}
using ProjectSims.Scripts.Entities;
using ProjectSims.Scripts.StateMachine;
using UnityEngine;

namespace ProjectSims.Scripts.PadestrianActivity
{
    public class PadestrianWanderingAround:IState<Work>
    {
        public void OnStateEnter(Work t)
        {
            Debug.Log("Change Work");
        }

        public void OnStateUpdate(Work t)
        {
            Debug.Log("Pdestrian Walking!");   
        }

        public void OnStateExit(Work t)
        {
            
        }
    }
}
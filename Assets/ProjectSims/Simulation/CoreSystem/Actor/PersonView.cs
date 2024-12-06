using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.Products;
using UnityEngine;
using UnityEngine.AI;

namespace Simulation
{
    public class PersonView : MonoBehaviour
    {
        private Person _controller;
        
        [Header("Fallback Data")]
        [field: SerializeField]
        public ProductSO MinDrink { get; private set; }

        [field: SerializeField] 
        public ProductSO MinFood { get; private set; }

        [field: SerializeField] public NavMeshAgent Agent { get; private set; }

        private void Start()
        {
            _controller = new Person(this);
            // _controller.ChangeActivity(ActivityType.SearchADrink);
            _controller.ChangeActivity(ActivityType.Walking);
        }

        public void UpdateController(float dt)
        {
            _controller.Update(dt);
        }
    }
}
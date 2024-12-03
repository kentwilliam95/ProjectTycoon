using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation.Stalls;

namespace Simulation
{
    public class CoreController : MonoBehaviour
    {
        public static CoreController Instance { get; private set; }
        
        private List<Person> _listPeople;
        [field: SerializeField] public Stall Stall { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _listPeople = new List<Person>();

            Person p = new Person();
            _listPeople.Add(p);
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            for (int i = 0; i < _listPeople.Count; i++)
            {
                _listPeople[i].Update(dt);
            }
        }

        public Stall GetStallNearPerson(Person person)
        {
            return Stall;
        }
    }
}
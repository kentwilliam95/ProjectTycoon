using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.Products;
using UnityEngine;
using Simulation.Stalls;

namespace Simulation
{
    public class CoreController : MonoBehaviour
    {
        public static CoreController Instance { get; private set; }
        private List<PersonView> _listPeople;
        [field: SerializeField] public Stall Stall { get; private set; }
        [field: SerializeField] private PersonView[] _personView;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _listPeople = new List<PersonView>();
            Stall.Initialize();
            
            _personView = GetComponentsInChildren<PersonView>();
            _listPeople.AddRange(_personView);
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            for (int i = 0; i < _listPeople.Count; i++)
            {
                _listPeople[i].UpdateController(dt);
            }
        }

        public Stall GetStallNearPerson(Person person)
        {
            return Stall;
        }
    }
}
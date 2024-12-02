using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
    public class CoreController : MonoBehaviour
    {
        private List<Person> _listPeople;
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
    }   
}

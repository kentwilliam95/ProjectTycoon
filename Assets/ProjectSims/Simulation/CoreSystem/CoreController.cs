using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.Products;
using UnityEngine;
using Simulation.Stalls;
using Simulation.UI;

namespace Simulation
{
    public class CoreController : MonoBehaviour
    {
        public static CoreController Instance { get; private set; }
        private List<PersonView> _listPeople;
        
        private TimeModule _time;
        public static float DayProgression;
        
        [field: SerializeField] public Stall Stall { get; private set; }
        [field: SerializeField] private PersonView[] _personView;

        [Header("User Interface")] [SerializeField]
        private UITimeController _uiTimeController;

        private void Awake()
        {
            Instance = this;
            _time = new TimeModule(0);
            _time.Speed = 1;
        }

        private void Start()
        {
            _listPeople = new List<PersonView>();
            Stall.Initialize();

            var length = _uiTimeController.ButtonsSpeed.Length;
            for (int i = 0; i < length; i++)
            {
                var pow = (int)Mathf.Pow(10, i + 1);
                _uiTimeController.SetButton(i, ((i+1) * 10) + "x", () => { SetTimeSpeed((i + 1) * pow);});
            }

            _personView = GetComponentsInChildren<PersonView>();
            _listPeople.AddRange(_personView);
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            
            _time.Update(dt);
            _uiTimeController.SetText(_time.ToString());
            DayProgression = _time.DayProgression;
            
            for (int i = 0; i < _listPeople.Count; i++)
            {
                _listPeople[i].UpdateController(dt);
            }
        }

        public Stall GetStallNearPerson(Person person)
        {
            return Stall;
        }

        public void SetTimeSpeed(int speed)
        {
            _time.Speed = speed;
        }
    }
}
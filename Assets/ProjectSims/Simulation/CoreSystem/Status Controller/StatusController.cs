using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
    public class StatusController
    {
        public enum Stats
        {
            Energy,
            Hunger,
            Health,
            WellBeing,
            Money,
            Bladder,
            Social,
            Fun,
            Thirsty,
            Urgency
        }

        private Dictionary<Stats, float> _dictStats;
        public StatusController()
        {
            _dictStats = new Dictionary<Stats, float>();
            
            _dictStats.Add(Stats.Energy, 100);
            _dictStats.Add(Stats.Hunger, 100);
            _dictStats.Add(Stats.Health, 100);
            _dictStats.Add(Stats.WellBeing, 100);
            _dictStats.Add(Stats.Money, 100);
            _dictStats.Add(Stats.Bladder, 100);
            _dictStats.Add(Stats.Social, 100);
            _dictStats.Add(Stats.Fun, 100);
            _dictStats.Add(Stats.Thirsty, 100);
            
            //100 : high urgency
            _dictStats.Add(Stats.Urgency, 0); 
        }

        public void Add(Stats stats, float value)
        {
            if (!_dictStats.ContainsKey(stats)) { return; }
            _dictStats[stats] += value;
        }

        public float GetStat(Stats stat)
        {
            if (!_dictStats.ContainsKey(stat))
            {
                return float.MinValue;
            }
            return _dictStats[stat];
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Simulation.Products
{
    [CanEditMultipleObjects]
    [CreateAssetMenu(menuName = "Simulation/Item/Item")]
    public class Item: ScriptableObject
    {
        public enum Level
        {
            Level0,
            Level1,
            Level2,
            Level3,
        }

        public string Name;
        public Level Lvl;
        
        private void OnValidate()
        {
            Name = this.name;
        }
    }   
}

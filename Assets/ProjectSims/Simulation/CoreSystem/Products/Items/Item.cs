using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Products
{
    [CreateAssetMenu(menuName = "Simulation/Resources")]
    public class Item: ScriptableObject
    {
        public string Name;
        public int Units;

        private void OnValidate()
        {
            Name = this.name;
        }
    }   
}

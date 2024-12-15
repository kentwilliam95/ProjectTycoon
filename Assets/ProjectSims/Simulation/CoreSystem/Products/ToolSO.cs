using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Products
{
    
    [CreateAssetMenu(menuName = "Simulation/Item/Tool")]
    public class ToolSO : Item
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public float Wattage { get; private set; }
        [field: SerializeField] public float Durability { get; private set; }
        [field: SerializeField] public float DegradeValue { get; private set; }
        public bool IsBroken => Durability <= 0;

        public void Use()
        {
            Durability -= DegradeValue;
            Durability = Mathf.Max(Durability - DegradeValue, 0);
        }
    }
}


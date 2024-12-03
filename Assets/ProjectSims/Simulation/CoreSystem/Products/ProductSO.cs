using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;  

namespace Simulation.Products
{
    [CreateAssetMenu(menuName = "Simulation/Product")]
    public class ProductSO : ScriptableObject
    {
        [System.Serializable]
        public struct Requirement
        {
            public Item Resource;
            public int Qty;
        }
        
        public string Name;
        public Requirement[] Requirements;
    }

    public class Product
    {
        
    }
}
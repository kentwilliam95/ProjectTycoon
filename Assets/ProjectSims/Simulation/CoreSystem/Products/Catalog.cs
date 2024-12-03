using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Products
{
    [CreateAssetMenu(menuName = "Simulation/Catalog")]
    public class Catalog : ScriptableObject
    {
        [System.Serializable]
        public struct Entrie
        {
            public ProductSO Product;
            public float Price;
        }

        [field: SerializeField] public List<Entrie> Entries { get; private set; }
    }   
}

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
        
        [System.Serializable]
        public struct AffectedStatsDetail
        {
            public StatusController.Stats Affected;
            public int Value;
        }
        
        public string Name;
        public Requirement[] Requirements;
        
        public AffectedStatsDetail[] AffectedStats;
    }

    public class Product
    {
        private ProductSO _productSo;
        private Person _person;

        public string Name => _productSo.Name;
        
        public Product(Person person,ProductSO productSo)
        {
            _productSo = productSo;
            _person = person;
        }

        public void Use()
        {
            if (_person == null) { return; }
            Debug.Log($"Use Product: {_productSo.Name}");
            
            foreach (var affected in _productSo.AffectedStats)
            {
                _person.StatusController.Add(affected.Affected, affected.Value);
            }
        }
    }
}
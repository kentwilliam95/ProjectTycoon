using System.Collections;
using System.Collections.Generic;
using Simulation.Inventory;
using UnityEngine;

namespace Simulation.Products
{
    [CreateAssetMenu(menuName = "Simulation/Product")]
    public class ProductSO : Item
    {
        [System.Serializable]
        public struct Requirement
        {
            public Item Resource;
            public float Qty;
        }

        [System.Serializable]
        public struct AffectedStatsDetail
        {
            public StatusController.Stats Affected;
            public int Value;
        }

        public Requirement[] Requirements;
        public AffectedStatsDetail[] AffectedStats;
        public ToolSO[] ToolsRequirements;

        public bool MakeProduct(InventoryController inventory, int howMany)
        {
            bool isValid = inventory.CheckItemForProduct(this);
            bool isToolValid = inventory.CheckTools(this);
            if (!isValid || !isToolValid)
            {
                Debug.Log("[Product] not enough resource to make this product!");
                return isValid;
            }

            for (int i = 0; i < Requirements.Length; i++)
            {
                var req = Requirements[i];
                inventory.Get(req.Resource, req.Qty);
            }

            inventory.UseTools(this);
            inventory.Add(this, 1);
            return true;
        }

        public void ConvertDown(InventoryController inventory)
        {
            if (!inventory.CheckItemQty(this, 1))
            {
                Debug.Log("[Product] not enough resource to convert to lower level");
                return;
            }

            for (int i = 0; i < Requirements.Length; i++)
            {
                var req = Requirements[i];
                inventory.Add(req.Resource, req.Qty);
            }
        }

        public bool IsContainStat(StatusController.Stats stat)
        {
            bool isValid = false;
            for (int i = 0; i < AffectedStats.Length; i++)
            {
                var aff = AffectedStats[i];
                if (aff.Affected == stat)
                {
                    isValid = true;
                    break;
                }
            }

            return isValid;
        }


        [Button("CheckRequirements")]
        public void CheckRequirements()
        {
            for (int i = 0; i < Requirements.Length; i++)
            {
                var req = Requirements[i];
                if (req.Resource.Lvl < Lvl - 1)
                {
                    Debug.Log($"Invalid Level! {req.Resource.Name}");
                }
            }
        }
    }

    public class EndProduct
    {
        private ProductSO _productSo;

        public string Name => _productSo.Name;

        public EndProduct(Person person, ProductSO productSo)
        {
            _productSo = productSo;
        }

        public void Use(Person _person)
        {
            if (_person == null)
            {
                return;
            }

            Debug.Log($"Use Product: {_productSo.Name}");

            foreach (var affected in _productSo.AffectedStats)
            {
                _person.StatusController.Add(affected.Affected, affected.Value);
            }
        }
    }
}
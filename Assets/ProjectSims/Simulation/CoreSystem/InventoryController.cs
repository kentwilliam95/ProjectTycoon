using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simulation.Products;
using UnityEngine;

namespace Simulation.Inventory
{
    public class InventoryController
    {
        private Dictionary<Item, float> _dictInventory;

        public InventoryController()
        {
            _dictInventory = new Dictionary<Item, float>(8);
        }

        public void Add(Item item, float qty)
        {
            var boolean = _dictInventory.TryAdd(item, qty);
            if (!boolean)
            {
                Debug.LogError("Add Ingredient Fail!");
            }
        }

        public bool Get(Item item, float amount)
        {
            if (!_dictInventory.ContainsKey(item))
            {
                return false;
            }

            if (!CheckItem(item, amount))
            {
                return false;
            }

            amount = amount > 0 ? -amount : amount;
            Add(item, amount);
            return true;
        }

        public bool CheckItem(Item item, float amount)
        {
            if (!_dictInventory.ContainsKey(item))
            {
                return false;
            }

            var stockQty = _dictInventory[item];
            if (stockQty - amount < 0)
            {
                return false;
            }

            return true;
        }

        public bool CheckItemForProduct(ProductSO productSo)
        {
            var requirement = productSo.Requirements;
            bool isValid = true;
            for (int i = 0; i < requirement.Length; i++)
            {
                var qty = requirement[i].Qty;
                var resource = requirement[i].Resource;
                if (!CheckItem(resource, qty))
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }
    }
}
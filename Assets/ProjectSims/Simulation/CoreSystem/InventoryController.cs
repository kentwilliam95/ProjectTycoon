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
        private Dictionary<Item, float> _dictInventory = new Dictionary<Item, float>(8);

        public void Add(Item item, float qty)
        {
            bool isContain = _dictInventory.ContainsKey(item);
            if (!isContain)
            {
                _dictInventory.Add(item, qty);
            }
            else
            {
                _dictInventory[item] += qty;
                Debug.Log($"Modify Item {item.Name}: {_dictInventory[item]}");
            }
        }

        public bool Get(Item item, float amount)
        {
            if (!_dictInventory.ContainsKey(item))
            {
                return false;
            }

            if (!CheckItemQty(item, amount))
            {
                return false;
            }

            amount = amount > 0 ? -amount : amount;
            Add(item, amount);
            return true;
        }

        public void UseTools(ProductSO product)
        {
            var tools = product.ToolsRequirements;
            for (int i = 0; i < tools.Length; i++)
            {
                bool isAvailable = _dictInventory.TryGetValue(tools[i], out float value);
                if (isAvailable)
                {
                    value -= tools[i].DegradeValue;
                    _dictInventory[tools[i]] = value;
                }
                else
                {
                    Debug.Log("Not enough tools");
                }
            }
        }

        public bool CheckItemQty(Item item, float amount)
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

        public bool CheckTools(ProductSO product)
        {
            var tools = product.ToolsRequirements;
            bool isValid = true;
            for (int i = 0; i < tools.Length; i++)
            {
                if (!_dictInventory.ContainsKey(tools[i]))
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }

        public bool CheckItemForProduct(ProductSO productSo)
        {
            var requirement = productSo.Requirements;
            bool isValid = true;
            for (int i = 0; i < requirement.Length; i++)
            {
                var qty = requirement[i].Qty;
                var resource = requirement[i].Resource;
                if (!CheckItemQty(resource, qty))
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }
    }
}
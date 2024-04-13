using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ProjectSims.Scripts.General;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectSims.Scripts.Place
{
    [CreateAssetMenu(menuName = "Place/Create Place", fileName = "Place SO")]
    public class PlaceSO : ScriptableObject
    {
        public class CustomerData
        {
            public int _customerID;
            public List<ItemBoughtHistory> _itemBoughtHistories;

            public CustomerData(int guid)
            {
                _itemBoughtHistories = new List<ItemBoughtHistory>();
                _customerID = guid;
            }

            public void GenerateHistory<T>(List<T> items, out int start, out int count) where T : ItemSO
            {
                start = _itemBoughtHistories.Count;
                for (int i = 0; i < items.Count; i++)
                {
                    var history = new ItemBoughtHistory();
                    history.ChangeItem(items[i]);
                    _itemBoughtHistories.Add(history);
                }

                count = _itemBoughtHistories.Count;
            }

            public void AddItem(ItemSO item, float weight = 0)
            {
                var history = GetItemHistory(item);
                if (history == null)
                {
                    history = new ItemBoughtHistory();
                    history.ChangeItem(item);
                    history.Add();
                    history.AddWeightValue(weight);
                    _itemBoughtHistories.Add(history);
                    return;
                }

                history.Add();
                history.AddWeightValue(-weight);
            }

            public ItemBoughtHistory GetItemHistory(ItemSO item)
            {
                for (int i = 0; i < _itemBoughtHistories.Count; i++)
                {
                    if (_itemBoughtHistories[i]._itemId == item.Guid)
                        return _itemBoughtHistories[i];
                }

                return null;
            }

            public List<ItemBoughtHistory> GetFullHistory()
            {
                return _itemBoughtHistories;
            }  
        }

        public class ItemBoughtHistory
        {
            public int _count;
            public float Weight;
            public ItemSO ItemSo { get; private set; }
            public int _itemId;

            public ItemBoughtHistory()
            {
                _count = 0;
                Weight = 100f;
            }

            public void ChangeItem(ItemSO item, bool resetCount = false)
            {
                _itemId = item.Guid;
                ItemSo = item;
                if (resetCount)
                    _count = 0;
            }

            public void Add(int amount = 1)
            {
                _count += amount;
            }

            public void AddWeightValue(float value)
            {
                Weight = Mathf.Max(0, Weight + value);
            }
        }

        public List<CustomerData> _customerData;
        protected Dictionary<int, CustomerData> _dictCustomerData;

        protected Place place;
        public Place Place => place;

        public virtual void Initialize()
        {
            _dictCustomerData = new Dictionary<int, CustomerData>();
            _customerData = new List<CustomerData>();
        }

        public void AddCustomer(Entity entity, CustomerData data)
        {
            _dictCustomerData.Add(entity.Guid, data);
            _customerData.Add(data);
        }

        public CustomerData GetCustomerData(int guid)
        {
            if (!_dictCustomerData.ContainsKey(guid))
                return default;

            return _dictCustomerData[guid];
        }

        public void BuyItem(Entity entity, ItemSO item, int amount) // for entity
        {
            //ToDo: track bought item
            int customerId = entity.Guid;
            bool isContains = _dictCustomerData.ContainsKey(customerId);
            CustomerData custData = default;

            // if (!isContains)
            // {
            //     custData = new CustomerData(customerId);
            //     custData.AddItem(item, Random.Range(0.025f, 0.75f));
            //     _dictCustomerData.Add(customerId, custData);
            //     _customerData.Add(custData);
            // }
            // else
            // {
                custData = _dictCustomerData[customerId];
                var history = custData.GetItemHistory(item);
                // if (history == null)
                // {
                //     custData.AddItem(item, Random.Range(0.025f, 0.75f));
                //     return;
                // }

                history.Add();
                history.AddWeightValue(-Random.Range(0.75f, 1.25f));
            // }
        }

        public ItemBoughtHistory GetItemHistory(int customerID, ItemSO itemSo)
        {
            var isContains = _dictCustomerData.ContainsKey(customerID);
            if (!isContains)
                return null;

            var history = _dictCustomerData[customerID].GetItemHistory(itemSo);
            if (history == null)
                return null;

            return history;
        }

        public virtual void Visit(Entity entity)
        {
            Place.Visit(entity);
        }
    }
}
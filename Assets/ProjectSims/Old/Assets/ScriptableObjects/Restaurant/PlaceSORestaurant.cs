using System.Collections.Generic;
using ProjectSims.Scripts;
using ProjectSims.Scripts.General;
using ProjectSims.Scripts.Place;
using UnityEditor;
using UnityEngine;

namespace ProjectSims.Assets.ScriptableObjects.Restaurant
{
    [CreateAssetMenu(menuName = "Place/Restaurant", fileName = "Place Restaurant")]
    public class PlaceSORestaurant : PlaceSO
    {
        [SerializeField] private FoodSO[] items;
        private List<FoodSO> _drinks;
        private List<FoodSO> _mains;

        private List<ItemBoughtHistory> _historyDrink;
        private List<ItemBoughtHistory> _historyFood;

        public override void Initialize()
        {
            base.Initialize();

            if (_drinks == null)
                _drinks = new List<FoodSO>();

            if (_mains == null)
                _mains = new List<FoodSO>();

            _mains.Clear();
            _drinks.Clear();
            for (int i = 0; i < items.Length; i++)
            {
                switch (items[i].FoodType)
                {
                    case FoodType.Drinks:
                        _drinks.Add(items[i]);
                        break;

                    case FoodType.Main:
                        _mains.Add(items[i]);
                        break;
                }
            }
        }

        public override void Visit(Entity entity)
        {
            Customer workCustomer = new Customer();
            workCustomer.Initialize(entity, this);
            entity.ChangeWork(workCustomer);
        }

        public List<ItemBoughtHistory> GetListFoodHistory(Entity entity)
        {
            if (_historyFood == null)
                _historyFood = new List<ItemBoughtHistory>();
            
            _historyFood.Clear();
            for (int i = 0; i < _mains.Count; i++)
            {
                var history = GetItemHistory(entity.Guid, _mains[i]);
                if (history != null)
                    _historyFood.Add(history);
                else
                {
                    // Todo: Optimize using object pooling
                    ItemBoughtHistory itemBoughtHistory = new ItemBoughtHistory();
                    itemBoughtHistory.Initialize(_mains[i]);
                    _historyFood.Add(itemBoughtHistory);
                }
            }

            return _historyFood;
        }

        public List<ItemBoughtHistory> GetListDrinkHistory(Entity entity)
        {
            if (_historyDrink == null)
                _historyDrink = new List<ItemBoughtHistory>();
            
            _historyDrink.Clear();
            for (int i = 0; i < _drinks.Count; i++)
            {
                var history = GetItemHistory(entity.Guid, _drinks[i]);
                if (history != null)
                    _historyDrink.Add(history);
                else
                {
                    // Todo: Optimize using object pooling
                    ItemBoughtHistory itemBoughtHistory = new ItemBoughtHistory();
                    itemBoughtHistory.Initialize(_drinks[i]);
                    _historyDrink.Add(itemBoughtHistory);
                }
            }
            return _historyDrink;
        }
    }
}
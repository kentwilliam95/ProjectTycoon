using System;
using UnityEngine;

namespace ProjectSims.Scripts.General
{
    [System.Serializable]
    public class Item
    {
        private Guid _guid;
        public Guid Guid => _guid;
        [field: SerializeField] public Guid ownerGuid { get; private set; }
        [field: SerializeField] public string GuidStr { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public float Price { get; private set; }
        [field: SerializeField] public string Description { get; private set; }

        public Item()
        {
            _guid = System.Guid.NewGuid();
            GuidStr = _guid.ToString();
        }

        public Item SetName(string name)
        {
            Name = name;
            return this;
        }

        public Item SetDescription(string description)
        {
            Description = description;
            return this;
        }

        public Item SetPrice(float price)
        {
            Price = price;
            return this;
        }

        public Item SetOwner(Guid guid)
        {
            ownerGuid = guid;
            return this;
        }
    }

    public class RestaurantMenuItem : Item
    {
        [field: SerializeField] public FoodType FoodType { get; private set; }
        [field: SerializeField] public Country FoodCountry { get; private set; }

        public RestaurantMenuItem SetFoodType(FoodType foodType)
        {
            FoodType = foodType;
            return this;
        }

        public RestaurantMenuItem SetFoodCountry(Country country)
        {
            FoodCountry = FoodCountry;
            return this;
        }
    }
}
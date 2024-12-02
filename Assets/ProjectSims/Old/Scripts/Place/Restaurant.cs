using System.Collections.Generic;
using ProjectSims.Scripts.General;
using UnityEngine;

namespace ProjectSims.Scripts.Place
{
    public class Restaurant: Place
    {
        public List<RestaurantMenuItem> food;
        public List<RestaurantMenuItem> drink;
        
        // public override void Initialize()
        // {
        //     base.Initialize();
        //     _name = "Restaurant";
        //     food = new List<RestaurantMenuItem>();
        //     drink = new List<RestaurantMenuItem>();
        // }

        public override void Visit(Entity entity)
        {
            // Customer workCustomer = new Customer();
            // workCustomer.Initialize(entity, this);
            // entity.ChangeWork(workCustomer);
        }

        // public void RegisterFoods(FoodSO[] foods)
        // {
        //     ListItem.Clear();
        //     for (int i = 0; i < foods.Length; i++)
        //     {
        //         var food = foods[i];
        //         RestaurantMenuItem item = new RestaurantMenuItem();
        //         item.SetDescription(food.Description).SetName(food.Name).SetPrice(food.Cost);
        //         item.SetFoodCountry(food.Country).SetFoodType(food.FoodType);
        //         GameController.RegisterItem(item);
        //         
        //         ListItem.Add(item.Guid);
        //     }
        // }

        // public override void Load(string data)
        // {
        //     base.Load(data);
        //     for (int i = 0; i < ListItem.Count; i++)
        //     {
        //         var item = GameController.GetItem(ListItem[i]);
        //         if (item == null)
        //             continue;
        //
        //         var restoItem = (RestaurantMenuItem)item;
        //         switch (restoItem.FoodType)
        //         {
        //             case FoodType.Drinks:
        //                 drink.Add(restoItem);
        //                 break;
        //             
        //             case FoodType.Main:
        //                 food.Add(restoItem);
        //                 break;
        //         }
        //     }
        // }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectSims.Assets.ScriptableObjects.Restaurant;
using ProjectSims.Scripts.Entities;
using ProjectSims.Scripts.Place;
using UnityEngine;
using ProjectSims.Scripts.StateMachine;

namespace ProjectSims.Scripts.ActivityInRestaurant
{
    public class ActivityCustomerWaiting : IState<Work>
    {
        private Buff debuff;
        private float seeOrderItemDuration;

        public void OnStateEnter(Work t)
        {
            seeOrderItemDuration = 1f;
            // debuff = new Buff(t.Entity, Attribute.Energy, 0.5f, 0.5f, -1);
            // debuff.onBuffEnd = OnDebuffEnd;
            // t.Entity.AddBuff(debuff);
            BuyFoodAndDrinks(t);
        }

        public void OnStateUpdate(Work t)
        {
            seeOrderItemDuration -= Time.deltaTime;
            if (seeOrderItemDuration <= 0)
            {
            }
        }

        public void OnStateExit(Work t)
        {
            if (debuff != null)
                t.Entity.RemoveBuff(debuff);
        }

        private void OnDebuffEnd()
        {
            Debug.Log("Debuff End,Restart!");
            debuff.Reset();
        }

        private void BuyFoodAndDrinks(Work t)
        {
            PlaceSORestaurant resto = (PlaceSORestaurant)t.Place;
            int money = (int)t.Entity.GetAttribute(Attribute.Money);
            var drinkHistory = resto.GetListDrinkHistory(t.Entity);
            var foodHistory = resto.GetListFoodHistory(t.Entity);
            var selectedItem1 = GetSelectiveItem(money, drinkHistory);
            var selectedItem2 = GetSelectiveItem(money, foodHistory);
        }

        private PlaceSO.ItemBoughtHistory GetSelectiveItem(int money, List<PlaceSO.ItemBoughtHistory> items)
        {
            items = items.Where(x => money - x.ItemSo.Cost > 0 && x.Weight > 10).ToList();
            Shuffle(items);
            float totalWeight = 0;
            for (int i = 0; i < items.Count; i++)
                totalWeight += items[i].Weight;

            var target = Random.Range(Mathf.Epsilon, 1f);
            float cumulativeWeight = 0;
            List<PlaceSO.ItemBoughtHistory> selectedItem = new List<PlaceSO.ItemBoughtHistory>();

            for (int i = 0; i < items.Count; i++)
            {
                cumulativeWeight += items[i].Weight / totalWeight;
                if (cumulativeWeight >= target)
                {
                    selectedItem.Add(items[i]);
                    money -= items[i].ItemSo.Cost;
                    return items[i];
                }
            }

            return null;
        }

        private void Shuffle<T>(List<T> list)
        {
            int count = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                int index = Random.Range(0, count);
                T hold = list[index];
                list[index] = list[i];
                list[i] = hold;
            }
        }
    }
}
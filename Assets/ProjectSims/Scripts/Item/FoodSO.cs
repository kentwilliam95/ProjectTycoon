using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSims.Scripts.General
{
    public enum Country
    {
        Indonesian,
        Japanese,
        Italian,
        Western,
    }

    public enum FoodType
    {
        Starter,
        Main,
        Dessert,
        Drinks
    }

    [CreateAssetMenu(menuName = "Item/FoodSO", fileName = "FoodSO")]
    public class FoodSO : ItemSO
    {
        public Country Country;
        public FoodType FoodType;
        public Attribute affected;
        public float amount;
    }
}
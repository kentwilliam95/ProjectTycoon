using System;
using ProjectSims.Scripts.General;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectSims.Scripts.Place
{
    public class PlaceViewRestaurant : PlaceView
    {
        public FoodSO[] _menu;
        public override void Initialize()
        {
            _place = new Restaurant();
            _place.Initialize();
        }

        public void RegisterFoods()
        {
            // ((Restaurant)_place).RegisterFoods(_menu);
        }
    }
}
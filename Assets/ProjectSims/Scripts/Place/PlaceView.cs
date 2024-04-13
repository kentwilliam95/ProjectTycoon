using System;
using UnityEngine;

namespace ProjectSims.Scripts.Place
{
    public class PlaceView: MonoBehaviour
    {
        protected Place _place;

        public Place Place => _place;

        public virtual void Initialize()
        {
            
        }

        public void VisitPlace(Entity entity)
        {
            _place.Visit(entity);
        }
        
        public void LoadData(string data)
        {
            if(string.IsNullOrEmpty(data))
                return;
            
            // _place?.Load(data);
        }

        // public string SaveData()
        // {
        //     return _place.Save();
        // }
    }
}
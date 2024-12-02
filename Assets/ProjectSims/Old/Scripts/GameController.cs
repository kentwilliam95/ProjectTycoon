using System;
using System.Buffers;
using System.Collections.Generic;
// using Newtonsoft.Json;
using ProjectSims.Scripts.General;
using ProjectSims.Scripts.Interface;
using ProjectSims.Scripts.Place;
using ProjectSims.Scripts.SaveData;
using UnityEngine;
using UnityEngine.Pool;

namespace ProjectSims.Scripts
{
    public class myclass
    {
        public string Name;
        public string Gon;
    }

    public class GameController : MonoBehaviour, ISaveLoad
    {
        public static string KeyGameData = "GameData";

        private static List<Item> _listItem;
        [SerializeField] private PlaceSO _place;
        private List<Entity> _listEntity;

        private void Start()
        {
            _listItem = new List<Item>(16);
            _listEntity = new List<Entity>(16);
            _place.Initialize();
            InitializeEntity();
        }

        private void InitializeEntity()
        {
            for (int i = 0; i < 8; i++)
                _listEntity.Add(new Entity());
            
            for (int i = 0; i < _listEntity.Count; i++)
            {
                if(_listEntity[i] != null)
                    _listEntity[i].ChangeWork(new Padestrian());
            }
        }

        private void Update()
        {
            for (int i = 0; i < _listEntity.Count; i++)
            {
                if(_listEntity[i] != null)
                    _listEntity[i].Update();
            }
        }

        public string Save()
        {
            return string.Empty;
        }

        public void Load(string data)
        {
            throw new NotImplementedException();
        }

        public static void RegisterItem(Guid ownerID, string name, string description, int price, out Guid guid)
        {
            Item item = new Item();
            item.SetName(name).SetDescription(description).SetPrice(price).SetOwner(ownerID);
            _listItem.Add(item);
            guid = item.Guid;
        }

        public static void RegisterItem(Item item)
        {
            if (_listItem.Contains(item))
            {
                Debug.Log($"Item {item} already exist!");
                return;
            }

            _listItem.Add(item);
        }

        public static void RegisterItem(RestaurantMenuItem item)
        {
            if (_listItem.Contains(item))
            {
                Debug.Log($"Item {item} already exist!");
                return;
            }

            _listItem.Add(item);
        }

        public static Item GetItem(Guid guid)
        {
            for (int i = 0; i < _listItem.Count; i++)
            {
                if (_listItem[i].Guid == guid)
                    return _listItem[i];
            }

            return null;
        }
    }
}
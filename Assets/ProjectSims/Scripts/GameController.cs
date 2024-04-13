using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ProjectSims.Scripts.General;
using ProjectSims.Scripts.Interface;
using ProjectSims.Scripts.Place;
using ProjectSims.Scripts.SaveData;
using UnityEngine;

namespace ProjectSims.Scripts
{
    public class GameController : MonoBehaviour, ISaveLoad
    {
        public static string KeyGameData = "GameData";

        private static List<Item> _listItem;
        [SerializeField] private PlaceSO _place;
        private Entity _entity;

        private void Start()
        {
            _listItem = new List<Item>();
            _entity = new Entity();
            _place.Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                for (int i = 0; i < 2; i++)
                    _place.Visit(_entity);   
            }

            _entity?.Update();
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
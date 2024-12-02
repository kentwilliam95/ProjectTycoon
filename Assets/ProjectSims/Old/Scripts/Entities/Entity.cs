using System;
using System.Collections.Generic;
using ProjectSims.Scripts.Entities;
using ProjectSims.Scripts.StateMachine;
using UnityEngine;

namespace ProjectSims.Scripts
{
    public enum Attribute
    {
        Hunger,
        Thirst,
        Energy,
        Bladder,
        Money
    }

    [System.Serializable]
    public class Entity
    {
        private static int Id = 500;
        [SerializeField] private string _guid;
        public int Guid;

        private List<Buff> _listBuff;
        private Dictionary<Attribute, float> _dictAttribute;
        protected Work _work;

        [field: SerializeField] public EntitySO Data { get; private set; }

        public Entity()
        {
            Guid = Id;
            _guid = Guid.ToString();
            
            _work = new Work();
            
            _listBuff = new List<Buff>();
            _dictAttribute = new Dictionary<Attribute, float>();

            _dictAttribute.Add(Attribute.Bladder, 100);
            _dictAttribute.Add(Attribute.Hunger, 100);
            _dictAttribute.Add(Attribute.Thirst, 100);
            _dictAttribute.Add(Attribute.Energy, 100);
            _dictAttribute.Add(Attribute.Money, 100);

            Id += 1;
        }

        public void Update()
        {
            UpdateBuff();
            _work?.Update();
        }

        private void UpdateBuff()
        {
            var dt = Time.deltaTime;
            for (int i = _listBuff.Count - 1; i >= 0; i--)
                _listBuff[i].Update(dt);
        }

        public void AddBuff(Buff buff)
        {
            _listBuff.Add(buff);
        }

        public void RemoveBuff(Buff buff)
        {
            bool isContains = _listBuff.Contains(buff);
            if (!isContains)
                return;

            _listBuff.Remove(buff);
        }

        public void UpdateStats(Attribute attribute, float value)
        {
            bool isContain = _dictAttribute.ContainsKey(attribute);
            if (!isContain)
                return;

            _dictAttribute[attribute] += value;
        }

        public float GetAttribute(Attribute attribute)
        {
            bool isContain = _dictAttribute.ContainsKey(attribute);
            if (!isContain)
                return default;

            return _dictAttribute[attribute];
        }

        public void ChangeWork(Work work)
        {
            _work = work;
            // Debug.Log($"Change Work to : {work.GetType().ToString()}");
        }

        public void LoadData(string data)
        {
            
        }

        public string SaveData()
        {
            return string.Empty;
        }
    }
}
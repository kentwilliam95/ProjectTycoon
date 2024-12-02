using System;
using UnityEngine;

namespace ProjectSims.Scripts.General
{
    [CreateAssetMenu(menuName = "Item/ItemSO", fileName = "ItemSO")]
    public class ItemSO : ScriptableObject
    {
        private void OnValidate()
        {
            Name = name;
        }

        public int Guid;
        public string Name;
        public string Description;
        public int Cost;
        public Sprite Sprite;
    }
}
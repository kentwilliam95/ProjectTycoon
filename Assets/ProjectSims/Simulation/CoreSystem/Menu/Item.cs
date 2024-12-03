using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Items
{
    [CreateAssetMenu(menuName = "Item", fileName = "Item")]
    public class Item : ScriptableObject
    {
        public string Name;
    }   
}

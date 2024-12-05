using System.Collections;
using System.Collections.Generic;
using Simulation.Products;
using UnityEngine;

namespace Simulation.Stalls
{
    [CreateAssetMenu(menuName = "Simulation/Stall Data", fileName = "Stall Data")]
    public class StallSO : ScriptableObject
    {
        [SerializeField] private Catalog _catalog;
        [SerializeField] private List<Item> _items;
    }   
}
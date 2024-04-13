using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSims.Scripts
{
    [CreateAssetMenu(menuName = "Data/Entity Data", fileName = "Entity Data")]
    public class EntitySO : ScriptableObject
    {
        public RangeValue _energyDeduction;
        public RangeValue _staringMoney;
    }
}
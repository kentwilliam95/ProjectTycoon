using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Simulation.Items;

namespace Simulation.Stalls
{
    public class Stall : MonoBehaviour
    {
        [field: SerializeField] public List<Item> Menu { get; private set; }

        public void GetMenuHistory() { }

        public void BuyMenu(System.Action<Item> onServeComplete)
        {
            Debug.Log("Thank you for buying in this stall!");
            StartCoroutine(MakeMenuForCustomer(onServeComplete));
        }

        private IEnumerator MakeMenuForCustomer(System.Action<Item> onComplete)
        {
            yield return new WaitForSeconds(2);
            onComplete?.Invoke(new Item());
        }
    }
}
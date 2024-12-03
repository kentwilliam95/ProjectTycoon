using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.Inventory;
using UnityEngine;
using Simulation.Products;
using UnityEditor;

namespace Simulation.Stalls
{
    public class Stall : MonoBehaviour
    {
        [System.Serializable]
        public struct DataWaiting
        {
            public int QueueNumber;
            public Person Person;
            public Action<Stall> OnCall;

            public void Reset()
            {
                Person = null;
                OnCall = null;
            }
        }

        private enum State
        {
            Idle,
            CallWaitingList,
            Serving
        }

        private InventoryController _inventory;
        private Coroutine _coroutineServe;
        private DataWaiting _serving;
        private State _state = State.Idle;
        private List<DataWaiting> _waitingList = new List<DataWaiting>(16);

        [SerializeField] private List<Item> _inventoryItems;

        public void Initialize()
        {
            _inventory = new InventoryController();
            foreach (var item in _inventoryItems)
            {
                Debug.Log($"Add Item:{item.Name} 99");
                _inventory.Add(item, 99);
            }
        }

        public void CustomerVisitStall(Person person, Action<Stall> onCall)
        {
            var dataQueue = new DataWaiting() { OnCall = onCall, Person = person, QueueNumber = _waitingList.Count };
            _waitingList.Add(dataQueue);
        }

        public void CustomerLeave(Person person)
        {
            var count = _waitingList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (_waitingList[i].Person != person)
                {
                    continue;
                }

                _waitingList.RemoveAt(i);
            }

            ResetServeState();
        }

        private void Update()
        {
            if (_waitingList.Count <= 0)
            {
                return;
            }
            
            switch (_state)
            {
                case State.Idle:
                    HandleIdle();
                    break;

                case State.CallWaitingList:
                    HandleCallWaitingList();
                    break;
            }
        }

        private void HandleIdle()
        {
            if (_waitingList.Count <= 0)
            {
                return;
            }

            //for now serve the 0 index, later this handle multiple request depends on idle employee
            _serving = _waitingList[0];
            
            _state = State.CallWaitingList;
        }

        private void HandleCallWaitingList()
        {
            if (_serving.Person == null || _serving.OnCall == null)
            {
                return;
            }

            _serving.OnCall?.Invoke(this);
            _state = State.Serving;
            Debug.Log("Calling!");
        }

        public void BuyMenu(System.Action<Stall, Product> onServeComplete)
        {
            Debug.Log("Please wait while your item is being process right now!");
            _coroutineServe = StartCoroutine(MakeMenuForCustomer(onServeComplete));
        }

        private IEnumerator MakeMenuForCustomer(System.Action<Stall, Product> onComplete)
        {
            yield return new WaitForSeconds(2);
            Debug.Log("Here is your Product sir/mam!");
            ResetServeState();
            onComplete?.Invoke(this, new Product());
        }

        private void ResetServeState()
        {
            if (_coroutineServe != null)
            {
                StopCoroutine(_coroutineServe);
            }

            _coroutineServe = null;
            _serving.Reset();
            _state = State.Idle;
        }
    }
}
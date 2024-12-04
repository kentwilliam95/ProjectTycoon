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
        [field: SerializeField] public List<ProductSO> Products { get; private set; }
        
        [TextArea(5,10)]
        public string _DebugLog;
        
        public void Initialize()
        {
            _inventory = new InventoryController();
            foreach (var item in _inventoryItems)
            {
                _inventory.Add(item, 999999);
            }
        }

        public void CustomerVisitStall(Person person, Action<Stall> onCall)
        {
            Debug.Log($"Visit Stall! {person}");
            var dataQueue = new DataWaiting() { OnCall = onCall, Person = person, QueueNumber = _waitingList.Count };
            _waitingList.Add(dataQueue);
        }

        public void CustomerLeave(Person person)
        {
            Debug.Log("Customer Leave!");
            var count = _waitingList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (_waitingList[i].Person == person)
                {
                    _waitingList.RemoveAt(i);
                    break;
                }
            }

            ResetServeState();
        }

        private void Update()
        {
            _DebugLog = $"Waiting List Count: {_waitingList.Count}";
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
            
            Debug.Log("Serving!");
            
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

        public void BuyMenu(ProductSO product, System.Action<Stall, Product> onServeComplete, System.Action<Stall, string> onFail)
        {
            Debug.Log("Please wait while your item is being process right now!");
            
            bool isValid = _inventory.CheckItemForProduct(product);
            if (isValid)
            {
                _coroutineServe = StartCoroutine(MakeMenuForCustomer(product, onServeComplete));   
            }
            else
            {
                onFail?.Invoke(this, "Sorry we could not advance your order, we don't have enough ingredients.");
                ResetServeState();
            }
        }

        private IEnumerator MakeMenuForCustomer(ProductSO product, System.Action<Stall, Product> onComplete)
        {            
            yield return new WaitForSeconds(2);
            Debug.Log("Here is your Product sir/mam!");
            onComplete?.Invoke(this, new Product(_serving.Person, product));
            ResetServeState();
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
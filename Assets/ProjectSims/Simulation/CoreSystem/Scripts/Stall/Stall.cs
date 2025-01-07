using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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
            public Person Person;
            public bool IsArrive;
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
            WaitingForCustomer,
            Serving
        }

        private InventoryController _inventory;
        private Coroutine _coroutineServe;
        private DataWaiting _serving;
        private State _state = State.Idle;
        private List<DataWaiting> _waitingList = new List<DataWaiting>(16);

        [SerializeField] private List<Item> _inventoryItems;
        [SerializeField] private Catalog _catalog;

        [field: SerializeField] public Transform OrderPoint { get; private set; }

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
            // Debug.Log($"Visit Stall! {person}");
            var dataQueue = new DataWaiting() { OnCall = onCall, Person = person };
            _waitingList.Add(dataQueue);
        }

        public void CustomerLeave(Person person)
        {
            // Debug.Log("Customer Leave!");
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

            //Todo: for now serve the 0 index, later this handle multiple request depends on idle employee
            _serving = _waitingList[0];
            _state = State.CallWaitingList;
        }

        private void HandleCallWaitingList()
        {
            if (_serving.Person == null || _serving.OnCall == null)
            {
                return;
            }

            var count = _waitingList.Count;
            for (int i = 0; i < count; i++)
            {
                var pos = OrderPoint.position - i * OrderPoint.forward;
                var speed = Person.WALKSPEED * 2;
                Action onArriveAtOrderPoint = i == 0
                    ? () =>
                    {
                        _serving.OnCall?.Invoke(this);
                        _state = State.Serving;
                    }
                    : null;

                _waitingList[i].Person.StartMoveTo(pos, speed, onArriveAtOrderPoint);
            }
            
            _state = State.WaitingForCustomer;
        }

        public void BuyMenu(ProductSO product, Action<Stall, EndProduct> onServeComplete, Action<Stall, string> onFail)
        {
            _state = State.Serving;
            bool isValid = product.MakeProduct(_inventory, 1);
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

        public List<ProductSO> GetProductBaseOnStats(StatusController.Stats stat)
        {
            //Todo: instead creating list every request, try pooling 
            var customList = new List<ProductSO>();
            for (int i = 0; i < _catalog.Entries.Count; i++)
            {
                var p = _catalog.Entries[i].Product;
                if (!p.IsContainStat(stat))
                {
                    continue;
                }

                customList.Add(p);
            }

            return customList;
        }

        public bool IsProductAvailable(ProductSO product)
        {
            return _inventory.CheckItemForProduct(product);
        }

        private IEnumerator MakeMenuForCustomer(ProductSO product, System.Action<Stall, EndProduct> onComplete)
        {
            //Todo: this is not fixed 2 seconds, it depends on stall equipment and worker condition
            _inventory.Get(product, 1);
            yield return new WaitForSeconds(2);
            onComplete?.Invoke(this, new EndProduct(_serving.Person, product));
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
using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Simulation.GroundEditor
{
    public class UIInputController : UiBase, IDragHandler, IPointerDownHandler, IPointerUpHandler, IScrollHandler
    {
        private enum State
        {
            None = 0,
            Single = 1,
            Double = 2
        }
        
        [SerializeField] private float _radius = 150f;
        [SerializeField] private float _clickDelay = 0.3f;
        [SerializeField] private Image[] _pointer;
        [SerializeField] private bool _showController;

        private Vector2 _startPos;
        private Vector2 _direction;
        private float _timeClickTracker;
        private State _pointerState = State.None;
      
        public Action<Vector3> OnClick;
        public Action<Vector3> OnUpdate;
        public Action<Vector2> OnScrolling;
        public Action<float> OnPinch;
        public Action OnPointerRelease;
        
        private Dictionary<int, Vector2> _dictPointer;
        
        [Header("Settings")]
        private float _initPinchSize;
        [SerializeField] private float _pinchSensitivity = 100;
        
        public Vector2 Center => new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        private void Start()
        {
            foreach (var pointer in _pointer)
            {
                pointer.gameObject.SetActive(_showController);
            }

            _dictPointer = new Dictionary<int, Vector2>();
            _pinchSensitivity = _pinchSensitivity * _canvas.scaleFactor;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_dictPointer.ContainsKey(eventData.pointerId))
            {
                _dictPointer[eventData.pointerId] = eventData.position;
            }

            switch (_pointerState)
            {
                case State.Double:
                    var currentSize = GetPinchSize();
                    var distance = currentSize - _initPinchSize;
                    OnPinch?.Invoke(distance/ _pinchSensitivity);
                    
                    int count = 0;
                    foreach (var dict in _dictPointer)
                    {
                        _pointer[count].transform.position = dict.Value;
                        count += 1;
                    }
                    break;
                
                case State.Single:
                    var diff = (_startPos - GetMidPointFromDictionary());
                    _direction = (eventData.position - _startPos).normalized;
                    var dist = diff.magnitude;
                    if (dist >= _radius)
                    {
                        _startPos = eventData.position - _direction * _radius;
                    }
                    break;
            }
        }

        private void Update()
        {
            if (_dictPointer.Count <= 0)
            {
                return;
            }

            Vector2 midPos = GetMidPointFromDictionary();
            midPos /= _dictPointer.Count;
            
            var diff = _startPos - midPos;
            var dist = diff.magnitude;
            OnUpdate?.Invoke(_direction * dist / _radius);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _dictPointer.TryAdd(eventData.pointerId, eventData.position);
            SetupState();

            switch (_pointerState)
            {
                case State.Single:
                    _timeClickTracker = Time.time;
                    break;
                
                case State.Double:
                    _initPinchSize = GetPinchSize();
                    break;
            }

            _startPos = GetMidPointFromDictionary();
            _direction = Vector2.zero;
            
            for (int i = 0; i < _pointer.Length; i++)
            {
                _pointer[i].transform.position = eventData.position;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_dictPointer.ContainsKey(eventData.pointerId))
            {
                _dictPointer.Remove(eventData.pointerId);
            }

            switch (_pointerState)
            {
                case State.Single:
                    var delayClick = Time.time - _timeClickTracker;
                    if (delayClick < _clickDelay)
                    {
                        OnClick?.Invoke(eventData.position);
                    }
                    break;
                
                case State.Double:
                    break;
            }

            _direction = Vector2.zero;
            OnPointerRelease?.Invoke();
            
            for (int i = 0; i < _pointer.Length; i++)
            {
                _pointer[i].transform.position = eventData.position;
            }
        }

        public void OnScroll(PointerEventData eventData)
        {
            OnScrolling?.Invoke(eventData.scrollDelta);
        }

        private float GetPinchSize()
        {
            Vector2 diff = Vector3.zero;
            foreach (var data in _dictPointer)
            {
                diff = data.Value - diff;
            }

            return diff.magnitude;
        }
        
        private void SetupState()
        {
            var count = _dictPointer.Count;
            switch (count)
            {
                case 0:
                    _pointerState = State.None;
                    break;

                case 1:
                    _pointerState = State.Single;
                    break;

                case 2:
                case 3:
                    _pointerState = State.Double;
                    break;
            }
        }

        private Vector2 GetMidPointFromDictionary()
        {
            Vector2 midPos = Vector2.zero;
            foreach (var pointer in _dictPointer)
            {
                midPos += pointer.Value;
            }

            return midPos / _dictPointer.Count;
        }
    }
}
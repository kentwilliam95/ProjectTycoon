using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Simulation.GroundEditor
{
    public class UIInputController : UiBase, IDragHandler,IPointerDownHandler,IPointerUpHandler,IScrollHandler
    {
        [SerializeField] private float _radius = 150f;
        [SerializeField] private float _clickDelay = 0.3f;
        [SerializeField] private Image _inner;
        [SerializeField] private Image _outer;
        [SerializeField] private bool _showController;
        
        private Vector2 _startPos;
        private Vector2 _direction;
        private bool _isPointerDown;
        private float _timeClickTracker;
        
        public Action<Vector3> OnDragging;
        public Action<Vector3> OnClick;
        public Action<Vector3> OnUpdate;
        public Action<Vector2> OnScrolling;
        public Action OnPointerRelease;

        private void Start()
        {
            _inner.gameObject.SetActive(_showController);
            _outer.gameObject.SetActive(_showController);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var diff = (_startPos - eventData.position);
            _direction = (eventData.position - _startPos).normalized;
            var dist = diff.magnitude;
            if (dist >= _radius)
            {
                _startPos = eventData.position - _direction * _radius;
                _inner.transform.position = _startPos;
            }

            _outer.transform.position = eventData.position;
            // OnDragging?.Invoke(_direction * dist/ _radius);
            OnDragging?.Invoke(eventData.position);
        }

        private void Update()
        {
            if (!_isPointerDown) { return;}
            var diff = _startPos - (Vector2)_outer.transform.position;
            var dist = diff.magnitude;
            OnUpdate?.Invoke(_direction * dist/ _radius);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log(eventData.pointerId);
            _timeClickTracker = Time.time;
            _startPos = eventData.position;
            _inner.transform.position = eventData.position;

            _direction = Vector2.zero;
            _isPointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var delayClick = Time.time - _timeClickTracker;
            if (delayClick < _clickDelay)
            {
                OnClick?.Invoke(eventData.position);
            }
            _direction = Vector2.zero;
            _inner.transform.position = eventData.position;
            _outer.transform.position = eventData.position;
            _isPointerDown = false;
            
            OnPointerRelease?.Invoke();
        }

        public void OnScroll(PointerEventData eventData)
        {
            Debug.Log("Scrolling!");
            Debug.Log(eventData.scrollDelta);
            OnScrolling?.Invoke(eventData.scrollDelta);
        }
    }   
}
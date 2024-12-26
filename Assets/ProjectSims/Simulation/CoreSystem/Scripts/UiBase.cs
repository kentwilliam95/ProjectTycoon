using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.UI
{
    public class UiBase : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup _cg;
        [SerializeField] protected Canvas _canvas;

        private void OnValidate()
        {
            if (_canvas == null)
            {
                _canvas = GetComponent<Canvas>();
            }
        }

        public virtual void Show()
        {
            _cg.alpha = 1;
            _cg.blocksRaycasts = true;
        }

        public virtual void Hide()
        {
            _cg.alpha = 0;
            _cg.blocksRaycasts = false;
        }
    }   
}
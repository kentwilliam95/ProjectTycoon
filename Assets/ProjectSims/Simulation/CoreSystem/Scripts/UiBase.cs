using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        
        private Sequence _sequence;

        private void OnDestroy()
        {
            _sequence?.Kill();
        }

        private void InitAnimation()
        {
            if (_sequence != null) { return;}

            _sequence = DOTween.Sequence();
            _sequence.SetAutoKill(false);
            
            _sequence.Insert(0f, _cg.DOFade(1f, 0.25f).From(0f));
            _sequence.Pause();
        }

        public virtual void Show()
        {
            InitAnimation();
            _cg.blocksRaycasts = true;
            _sequence.timeScale = 1f;
            _sequence.Restart();
        }

        public virtual void Hide(bool instant = false)
        {
            InitAnimation();
            _cg.blocksRaycasts = false;

            if (instant)
            {
                _sequence.timeScale = 4f;
                _sequence.SmoothRewind();    
            }
            else
            {
                _sequence.Rewind();
            }
            
        }
    }   
}
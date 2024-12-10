using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.UI
{
    public class UiBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _cg;
        public virtual void Show()
        {
            _cg.alpha = 1;
            _cg.blocksRaycasts = true;
        }

        public virtual void Hide()
        {
            _cg.alpha = 0;
            _cg.blocksRaycasts = true;
        }
    }   
}
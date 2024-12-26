using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulation.GroundEditor
{
    public class UIBuildItemTemplate : MonoBehaviour
    {
        private DecorationSO.AssetDetail _so;
        private Action<DecorationSO.AssetDetail> _onClicked;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _title;
        
        public void Init(DecorationSO.AssetDetail so, Action<DecorationSO.AssetDetail> onButtonClicked)
        {
            _so = so;
            _onClicked = onButtonClicked;
            
            _title.SetText(so.Template.name);
            _icon.sprite = so.Sprite;
        }

        public void ButtonBuyClicked()
        {
            _onClicked?.Invoke(_so);
        }
    }   
}

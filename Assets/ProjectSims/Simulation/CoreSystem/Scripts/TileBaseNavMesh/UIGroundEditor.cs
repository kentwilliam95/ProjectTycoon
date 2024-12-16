using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulation.GroundEditor
{
    public class UIGroundEditor : UiBase
    {
        [SerializeField] private CanvasGroup _goMenu;
        [SerializeField] private CanvasGroup _goSelection;
        [SerializeField] private CanvasGroup _goControl;
        [SerializeField] private Button _buttonReplace;
        [SerializeField] private TextMeshProUGUI _textTitle;
        [SerializeField] private Slider _sliderZoom;
        
        public Action<float> OnZoomChange;

        private void Start()
        {
            _sliderZoom.onValueChanged.AddListener(Slider_OnZoomValueChanged);
        }

        public void EnableControls()
        {
            _goControl.interactable = true;
        }

        public void DisableControls()
        {
            _goControl.interactable = false;
        }

        public void HighlightButtonReplace()
        {
            _buttonReplace.GetComponentInChildren<Image>().color = Color.yellow;
        }
        
        public void UnHighlightButtonReplace()
        {
            _buttonReplace.GetComponentInChildren<Image>().color = Color.white;
        }

        public void SetTitle(string text)
        {
            _textTitle.SetText(text);
        }

        public void DisableMenu()
        {
            _goMenu.interactable = false;
        }

        public void EnableMenu()
        {
            _goMenu.interactable = true;
        }

        public void DisableSelection()
        {
            _goSelection.interactable = false;
        }

        public void EnableSelection()
        {
            _goSelection.interactable = true;
        }

        private void Slider_OnZoomValueChanged(float value)
        {
            OnZoomChange?.Invoke(value);
        }
    }   
}
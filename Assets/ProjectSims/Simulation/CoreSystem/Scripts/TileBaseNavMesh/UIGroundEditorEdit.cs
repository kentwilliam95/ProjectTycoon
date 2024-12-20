using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulation.GroundEditor
{
    public class UIGroundEditorEdit : UiBase
    {
        [SerializeField] private CanvasGroup _goMenu;
        [SerializeField] private CanvasGroup _goSelection;
        [SerializeField] private CanvasGroup _goControl;
        [SerializeField] private Button _buttonReplace;
        [SerializeField] private Button _buttonDone;
        [SerializeField] private Button _buttonGrass;
        [SerializeField] private Button _buttonPavement;
        [SerializeField] private Button _buttonNew;
        [SerializeField] private Button _buttonLoad;
        [SerializeField] private Button _buttonEdit;
        
        [SerializeField] private TextMeshProUGUI _textTitle;
        [SerializeField] private Slider _sliderZoom;
        
        public Action<float> OnZoomChange;
        public Action OnButtonDoneClicked;
        public Action OnButtonPavementClicked;
        public Action OnButtonGrassClicked;
        public Action OnButtonNewClicked;
        public Action OnButtonLoadClicked;
        public Action OnButtonEditClicked;

        private void Start()
        {
            _sliderZoom.onValueChanged.AddListener(Slider_OnZoomValueChanged);
            _buttonDone.onClick.AddListener(ButtonDone_OnClicked);
            _buttonGrass.onClick.AddListener(ButtonGrass_OnClicked);
            _buttonPavement.onClick.AddListener(ButtonPavement_OnClicked);
            _buttonNew.onClick.AddListener(ButtonNew_OnClicked);
            _buttonLoad.onClick.AddListener(ButtonLoad_OnClicked);
            _buttonEdit.onClick.AddListener(ButtonEdit_OnClicked);
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

        private void ButtonPavement_OnClicked()
        {
            OnButtonPavementClicked?.Invoke();
        }
        
        private void ButtonDone_OnClicked()
        {
            OnButtonDoneClicked?.Invoke();
        }
        
        private void ButtonGrass_OnClicked()
        {
            OnButtonGrassClicked?.Invoke();
        }
        
        private void ButtonNew_OnClicked()
        {
            OnButtonNewClicked?.Invoke();
        }
        
        private void ButtonLoad_OnClicked()
        {
            OnButtonLoadClicked?.Invoke();
        }
        
        private void ButtonEdit_OnClicked()
        {
            OnButtonEditClicked?.Invoke();
        }
    }   
}
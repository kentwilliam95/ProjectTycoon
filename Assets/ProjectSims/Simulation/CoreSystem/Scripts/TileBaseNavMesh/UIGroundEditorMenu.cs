using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulation.GroundEditor
{
    public class UIGroundEditorMenu : UiBase
    {
        [SerializeField] private Button _buttonEditMode;
        [SerializeField] private Button _buttonBuildMode;
        
        public Action ButtonEditModeOnClicked;
        public Action ButtonBuildModeOnClicked;
        
        private void Start()
        {
            _buttonEditMode.onClick.AddListener(ButtonEditMode_OnClicked);
            _buttonBuildMode.onClick.AddListener(ButtonBuildMode_OnClicked);
        }

        private void ButtonEditMode_OnClicked()
        {
            ButtonEditModeOnClicked?.Invoke();
        }

        private void ButtonBuildMode_OnClicked()
        {
            ButtonBuildModeOnClicked?.Invoke();
        }
    }   
}

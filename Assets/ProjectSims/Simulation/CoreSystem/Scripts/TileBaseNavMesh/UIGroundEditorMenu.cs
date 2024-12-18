using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Simulation.GroundEditor
{
    public class UIGroundEditorMenu : UiBase
    {
        [SerializeField] private Button _buttonEditMode;

        public Action ButtonEditModeOnClicked;
        private void Start()
        {
            _buttonEditMode.onClick.AddListener(ButtonEditMode_OnClicked);
        }

        private void ButtonEditMode_OnClicked()
        {
            ButtonEditModeOnClicked?.Invoke();
        }
    }   
}

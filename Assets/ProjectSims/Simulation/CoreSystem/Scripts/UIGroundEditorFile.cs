using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulation.GroundEditor
{
    public class UIGroundEditorFile : UiBase
    {
        [SerializeField] private Button _buttonSave;
        [SerializeField] private Button _buttonCancel;

        [SerializeField] private TMP_InputField _inputSizeX;
        [SerializeField] private TMP_InputField _inputSizeY;

        public Action<int, int> OnButtonSaveClicked;
        private void Start()
        {
            _buttonSave.onClick.AddListener(ButtonSave_OnClicked);
            _buttonCancel.onClick.AddListener(ButtonCancel_OnClicked);
        }

        private void ButtonSave_OnClicked()
        {
            var x = System.Convert.ToInt32(_inputSizeY.text);
            var y = System.Convert.ToInt32(_inputSizeX.text);
            OnButtonSaveClicked?.Invoke(x, y);
            base.Hide();
        }

        private void ButtonCancel_OnClicked()
        {
            base.Hide();
        }
    }   
}
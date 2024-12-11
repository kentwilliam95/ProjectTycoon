using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Simulation.UI
{
    public class UITimeController : UiBase
    {
        [SerializeField] private TMP_Text _text;
        [field: SerializeField] public UIButtonExt[] ButtonsSpeed;

        private void OnValidate()
        {
            ButtonsSpeed = GetComponentsInChildren<UIButtonExt>();
        }

        public void SetButton(int index, string text, UnityAction action)
        {
            ButtonsSpeed[index].Text.SetText(text);
            ButtonsSpeed[index].Button.onClick.AddListener(action);
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }
    }
}
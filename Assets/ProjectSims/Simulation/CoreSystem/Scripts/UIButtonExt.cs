using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simulation.UI
{
    public class UIButtonExt : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
        [field: SerializeField] public Button Button { get; private set; }
        [SerializeField] private string _buttonText;
        private void OnValidate()
        {
            if (!Text)
            {
                Text = GetComponentInChildren<TextMeshProUGUI>();
            }

            if (!Button)
            {
                Button = GetComponentInChildren<UnityEngine.UI.Button>();
            }
            
            Text.SetText(_buttonText);
        }
    }
}
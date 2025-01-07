using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Simulation.UI
{
    public class UILoading : UiBase
    {
        public static UILoading Instance { get; private set; }
        [SerializeField] private RectTransform _spinner;
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            Instance = this;
        }

        public void Show(string text)
        {
            _text.SetText(text);
            base.Show();
        }

        private void Update()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            _spinner.Rotate(Vector3.forward, 90 * Time.deltaTime);
        }
    }
}
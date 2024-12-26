using System;
using System.Collections;
using System.Collections.Generic;
using Simulation.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Simulation.GroundEditor
{
    public class UIGroundEditorBuild : UiBase
    {
        private List<GameObject> _spawnedTemplate;
        [field: SerializeField] public TextMeshProUGUI Title { get; private set; }
        
        [SerializeField] private Button _buttonDone;
        public Button ButtonDone => _buttonDone;
        
        [SerializeField] private DecorationSO _decorationSo;
        [SerializeField] private RectTransform _itemContainer;
        [SerializeField] private UIBuildItemTemplate _template;
        
        [SerializeField] private Button _buttonMove;
        public Button ButtonMove => _buttonMove;
        
        [SerializeField] private Button _buttonRotate;
        public Button ButtonRotate => _buttonRotate;
        
        [SerializeField] private Button _buttonCheck;
        public Button ButtonCheck => _buttonCheck;
        
        [SerializeField] private Button _buttonDelete;
        public Button ButtonDelete => _buttonDelete;
        
        public Action OnButtonCheckClicked;
        public Action OnButtonMoveClicked;
        public Action OnButtonRotateClicked;
        public Action OnButtonDeleteClicked;
        
        public Action OnButtonDoneClicked;
        public Action<DecorationSO.AssetDetail> OnItemClicked;

        private void Start()
        {
            _spawnedTemplate = new List<GameObject>(8);
            _buttonDone.onClick.AddListener(ButtonDone_OnClicked);
            _buttonMove.onClick.AddListener(ButtonMove_OnClicked);
            _buttonCheck.onClick.AddListener(ButtonCheck_OnClicked);
            _buttonRotate.onClick.AddListener(ButtonRotate_OnClicked);
            _buttonDelete.onClick.AddListener(ButtonDelete_OnClicked);
        }

        public void Init()
        {
            base.Show();
            for (int i = _spawnedTemplate.Count - 1; i >= 0; i--)
            {
                Destroy(_spawnedTemplate[i].gameObject);
            }
            _spawnedTemplate.Clear();
            
            for (int i = 0; i < _decorationSo.Asset.Length; i++)
            {
                var go = Instantiate(_template, _itemContainer);
                go.Init(_decorationSo.Asset[i], OnItemClicked);
                go.gameObject.SetActive(true);
                _spawnedTemplate.Add(go.gameObject);
            }
        }

        private void ButtonDone_OnClicked()
        {
            OnButtonDoneClicked?.Invoke();
        }

        private void ButtonCheck_OnClicked()
        {
            OnButtonCheckClicked?.Invoke();
        }
        
        private void ButtonMove_OnClicked()
        {
            OnButtonMoveClicked?.Invoke();
        }
        
        private void ButtonRotate_OnClicked()
        {
            OnButtonRotateClicked?.Invoke();   
        }

        private void ButtonDelete_OnClicked()
        {
            OnButtonDeleteClicked?.Invoke();
        }
    }   
}

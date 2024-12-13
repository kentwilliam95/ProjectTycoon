using System;
using System.Collections;
using System.Collections.Generic;
using ProjectSims.Simulation.CoreSystem;
using Unity.AI.Navigation;
using UnityEngine;

namespace Simulation.GroundEditor
{
    [SelectionBase]
    public class GroundBox : MonoBehaviour
    {
        private float length = 1f;

        [SerializeField] private Transform _model;
        [SerializeField] private Renderer _renderer;
        private MaterialPropertyBlock _mbp;
        
        private bool _isSelected;
        public bool IsSelected => _isSelected;
        
        public Transform Model => _model;

        public Vector3 TopCenter => transform.position + Vector3.up + 0.5f * Vector3.right + 0.5f * Vector3.back;
        [field: SerializeField] public GroundArea.GroundType GroundType { get; private set; }
        [field: SerializeField] public Vector2Int Index { get; private set; }

        private void Awake()
        {
            _mbp = new MaterialPropertyBlock();
        }

        public void SetIndex(Vector2Int newIndex)
        {
            Index = newIndex;
        }

        [SerializeField] private Color _ogColor = new Color(0.1764705f,0.2039215f,0.2117647f);

        public void Select()
        {
            _mbp.SetColor("_BaseColor", Color.white);
            _renderer.SetPropertyBlock(_mbp, 0);
            _isSelected = true;
        }

        public void UnSelect()
        {
            _mbp.SetColor("_BaseColor", _ogColor);
            _renderer.SetPropertyBlock(_mbp, 0);
            _isSelected = false;
        }
    }
}
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
        public Transform Model => _model;

        public Vector3 TopCenter => transform.position + Vector3.up + 0.5f * Vector3.right + 0.5f * Vector3.back;
        [field: SerializeField] public GroundArea.GroundType GroundType { get; private set; }
        [field: SerializeField] public Vector2Int Index { get; private set; }

        public void SetIndex(Vector2Int newIndex)
        {
            Index = newIndex;
        }
    }
}
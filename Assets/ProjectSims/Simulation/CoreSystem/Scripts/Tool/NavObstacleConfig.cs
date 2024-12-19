using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Simulation.GroundEditor
{
    [RequireComponent(typeof(NavMeshObstacle))]
    public class NavObstacleConfig : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private NavMeshObstacle _navObstable;

        [SerializeField] private Vector3 _size;
        [SerializeField] private float _scale = 1f;
        
        private void OnValidate()
        {
            var center = _navObstable.center;
            center.y = _size.y / 2f;

            _navObstable.size = _size;
            _navObstable.center = center;
        }

        [Button("Setup Obstacle")]
        private void SetupObstacle()
        {
            if (!_meshFilter)
            {
                _meshFilter = GetComponentInChildren<MeshFilter>();
            }

            if (!_navObstable)
            {
                var comp = GetComponentInChildren<NavMeshObstacle>();
                _navObstable = comp;
            }

            var mesh = _meshFilter.sharedMesh;
            _meshFilter.transform.localScale = Vector3.one * _scale;
            var scaleFactor = _meshFilter.transform.localScale;
            _navObstable.center = Vector3.Scale(mesh.bounds.center, scaleFactor);
            _navObstable.size = Vector3.Scale(mesh.bounds.size , scaleFactor);
            _size = _navObstable.size;
        }
    }
}
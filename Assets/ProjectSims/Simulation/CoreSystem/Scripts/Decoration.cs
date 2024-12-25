using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Simulation.GroundEditor
{
    public class Decoration : MonoBehaviour
    {
        public string Name;

        [Header("BoxCast Collision Setting")] 
        public bool showIllustration = false;
        public Vector3 _extends;
        public float _length;
        
        private void Start()
        {
            var gos = GetComponentsInChildren<Transform>();
            foreach (var go in gos)
            {
                go.gameObject.layer = LayerMask.NameToLayer("Decoration");
            }
        }

        public void InitSpawnAnimation()
        {
            transform.DOScale(1f, 0.35f).From(0f).SetEase(Ease.OutBack);
        }

        public void Select()
        {
            transform.DOPunchScale(Vector3.one * -0.15f, 0.25f, 1, 1f).SetEase(Ease.OutBack);
        }

        public void Deselect() { }
        
        private void Reset()
        {
            Name = gameObject.name;
        }

        private void OnDrawGizmosSelected()
        {
            if (!showIllustration) { return;}
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position + new Vector3(0, _extends.y * 0.5f, 0), _extends);
            Gizmos.DrawWireCube(transform.position + new Vector3(0, _extends.y * 0.5f, 0) + Vector3.down * _length, _extends);
        }

        #if UNITY_EDITOR
        public Mesh Mesh;
        [Button]
        public void SetupBoxCast()
        {
            Vector3 localScale = Vector3.one;   
            
                var meshFilter = GetComponentInChildren<MeshFilter>();
                Mesh = meshFilter.sharedMesh;
                localScale = meshFilter.transform.localScale;
            

            _extends = Mesh.bounds.size;
            _extends = Vector3.Scale(_extends, localScale);
            _length = 2;
        }
        #endif
    }
}
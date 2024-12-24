using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Simulation.GroundEditor
{
    public class Decoration : MonoBehaviour
    {
        public string Name;

        private void Start()
        {
            var gos = GetComponentsInChildren<Transform>();
            foreach (var go in gos)
            {
                go.gameObject.layer = LayerMask.NameToLayer("Decoration");
            }
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
    }
}
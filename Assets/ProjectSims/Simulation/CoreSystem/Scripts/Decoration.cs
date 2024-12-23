using System;
using System.Collections;
using System.Collections.Generic;
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

        private void Reset()
        {
            Name = gameObject.name;
        }
    }   
}
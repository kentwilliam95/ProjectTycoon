using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.GroundEditor
{
    public class Decoration : MonoBehaviour
    {
        public string Name;

        private void Reset()
        {
            Name = gameObject.name;
        }
    }   
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Simulation.GroundEditor.Editor
{
    public class EditorDecoration : MonoBehaviour
    {
        [MenuItem("Window/SetupDecoration")]
        public static void SetupDecoration()
        {
            var gos = Selection.gameObjects;
            foreach (var go in gos)
            {
                var decor = go.GetComponent<Decoration>();
                if (!decor) { continue;}
                decor.SetupBoxCast();
                
            }
        }
    }   
}
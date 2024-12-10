using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
    public class AmbientController : MonoBehaviour
    {
        [SerializeField] private Gradient _skyColor;
        [SerializeField] private Gradient _equatorColor;

        [Header("Sun")] [SerializeField] private AnimationCurve _sunIntensity;
        [SerializeField] private Gradient _sunColor;
        [SerializeField] private Light _sun;
        private void Update()
        {
            var dayProgression = CoreController.DayProgression;
            var sky = _skyColor.Evaluate(dayProgression);
            var equator =  _equatorColor.Evaluate(dayProgression);

            RenderSettings.ambientSkyColor = sky;
            RenderSettings.ambientEquatorColor = equator;

            _sun.intensity = _sunIntensity.Evaluate(dayProgression);
            _sun.color = _sunColor.Evaluate(dayProgression);
        }
    }   
}
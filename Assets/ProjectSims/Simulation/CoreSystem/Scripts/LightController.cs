using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
    public class LightController : MonoBehaviour
    {
        [SerializeField] private Light _light;
        
        void Update()
        {
            var progress = CoreController.DayProgression;
            if (progress > 0.35f && progress < 0.75f)
            {
                _light.gameObject.SetActive(false);
            }
            else
            {
                _light.gameObject.SetActive(true);
            }
        }
    }   
}
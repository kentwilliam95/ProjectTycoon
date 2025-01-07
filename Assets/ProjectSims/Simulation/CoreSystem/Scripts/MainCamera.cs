using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Simulation
{
    public class MainCamera : MonoBehaviour
    {
        public static MainCamera Instance;
        [SerializeField] private Camera _cam;
        
        private float _range;
        private Tweener _tweenerCam;
        
        public Camera Camera => _cam;
        
        public float Orthographic
        {
            set
            {
                _cam.orthographicSize = value;
            }
            get => _cam.orthographicSize;
        }

        private void Awake()
        {
            Instance = this;
            _range = _cam.transform.position.magnitude;
        }

        public void MoveToTarget(Vector3 pos)
        {
            var fwd = _cam.transform.forward;

            var newPos = pos - fwd * _range;
            var initPos = _cam.transform.position;
            if (_tweenerCam != null && _tweenerCam.IsPlaying())
            {
                _tweenerCam.Kill();
            }

            _tweenerCam = DOVirtual.Vector3(initPos, newPos, 0.5f, (val) =>
            {
                _cam.transform.position = val; 
            });
        }

        public Ray ScreenPointToRay(Vector3 pos)
        {
            return _cam.ScreenPointToRay(pos);
        }
    }   
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _shadow;
    [SerializeField] private Light2D _light;
    
    [Range(0, 1)] 
    [SerializeField] private float _length;

    [SerializeField] private Vector3 _offset;
    private void LateUpdate()
    {
        var shadowTransform = _shadow.transform;
        var diff = _light.transform.position - shadowTransform.position;
        Vector3 toward = diff.normalized;

        shadowTransform.localPosition = (_offset - toward) * _length;
    }
}
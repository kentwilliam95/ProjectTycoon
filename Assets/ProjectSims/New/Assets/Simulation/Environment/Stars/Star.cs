using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Star : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _light;
    [SerializeField] private Gradient _color;

    [SerializeField] private float _lightIntensityMultiplier = 0.25f;
    private Color _defaultColor = Color.white;
    private Sequence _seqFade;
    
    private void OnValidate()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Start()
    {
        _seqFade = DOTween.Sequence();
        _seqFade.SetAutoKill(false);
    }

    public void TurnOff()
    {
        _light.color = Color.clear;
        _spriteRenderer.color = Color.clear;
    }

    public void TurnOn()
    {
        _light.color = _defaultColor;
        _spriteRenderer.color = _defaultColor;
    }

    public void FadeOff()
    {
        
    }

    public void FadeOn()
    {
        
    }

    public void SetIntensity(float intensity)
    {
         var color = _color.Evaluate(intensity);
        _spriteRenderer.color = color;

        color.a *= _lightIntensityMultiplier;
        _light.color = color;
    }
}

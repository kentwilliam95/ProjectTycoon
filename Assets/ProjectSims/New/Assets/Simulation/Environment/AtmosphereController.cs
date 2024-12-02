using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public interface IEnvironmentUpdater
{
   void UpdateControl(float progress);
}

public class AtmosphereController : MonoBehaviour, IEnvironmentUpdater
{
   private MaterialPropertyBlock _mbp;
   [SerializeField] private SpriteRenderer _spriteRenderer;
   [SerializeField] private Gradient _color;
   
   [Header("Controller")]
   [SerializeField] private AnimationCurve _curveInvert;
   [SerializeField] private AnimationCurve _gradientAmount;
   
   private static readonly int Invert = Shader.PropertyToID("_Intensity");
   private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
   private static readonly int VerticalAmount = Shader.PropertyToID("_VerticalAmount");

   private void Start()
   {
      _mbp = new MaterialPropertyBlock();
   }

   public void UpdateControl(float progress)
   {
      var invert = _curveInvert.Evaluate(progress);
      _mbp.SetFloat(Invert, invert);
      
      var color = _color.Evaluate(progress);
      _mbp.SetColor(BaseColor, color);

      var vertical = _gradientAmount.Evaluate(progress);
      _mbp.SetFloat(VerticalAmount, vertical);
      
      _spriteRenderer.SetPropertyBlock(_mbp, 0);
   }
}

using UnityEngine;
using System;

namespace ProjectSims.Scripts
{
    public class Buff
    {
        private float _countdownTick;
        private float _countdownDuration;
        protected float _duration;
        protected float _tick;
        protected float _dmg;
        private bool _isFinish;
        
        private Entity _ent;
        private Attribute _attributeAffected;

        public Action onUpdate;
        public Action onBuffEnd;
        
        public Buff(Entity ent, Attribute attribute, float duration, float tick, float dmg)
        {
            Initialize(duration, tick, dmg);
            
            _attributeAffected = attribute;
            _ent = ent;
        }

        public void Initialize(float duration, float tick, float dmg)
        {
            _duration = duration;
            _tick = tick;
            _dmg = dmg;
            
            _countdownTick = _tick;
            _countdownDuration = duration;
        }

        public virtual void Update(float dt)
        {
            if(_isFinish)
                return;
            
            _countdownDuration -= dt;
            if (_countdownDuration < 0)
            {
                _ent.UpdateStats(_attributeAffected, _dmg);
                _isFinish = true;
                
                onUpdate?.Invoke();
                onBuffEnd?.Invoke();
                return;   
            }
            
            _countdownTick -= dt;
            if (_countdownTick > 0)
                return;
            
            _ent.UpdateStats(_attributeAffected, _dmg);
            onUpdate?.Invoke();
            _countdownTick = _tick;
        }

        public void Reset()
        {
            _countdownDuration = _duration;
            _countdownTick = _tick;
            _isFinish = false;
        }
    }
}
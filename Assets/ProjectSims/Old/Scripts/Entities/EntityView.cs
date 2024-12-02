using System;
using System.Collections;
using System.Collections.Generic;
using ProjectSims.Scripts.ActivityInRestaurant;
using ProjectSims.Scripts.Place;
using UnityEngine;

namespace ProjectSims.Scripts
{
    public class EntityView : MonoBehaviour
    {
        private LayerMask _layerMaskPlace;
        private RaycastHit[] _hitResult;
        public Entity Entity { get; private set; }

        public void Initialize()
        {
            _layerMaskPlace = LayerMask.GetMask("Place");
            _hitResult = new RaycastHit[8];
            Entity = new Entity();
            
            Debug.Log("Entity View!");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                PlaceView pv = CollideWithPlace();
                if (pv != null)
                {
                    
                }
            }
            Entity.Update();
        }

        private PlaceView CollideWithPlace()
        {
            PlaceView pv = null;
            int count = Physics.SphereCastNonAlloc(transform.position, 2f, Vector3.up, _hitResult, 0f, _layerMaskPlace);
            for (int i = 0; i < count; i++)
            {
                 pv = _hitResult[i].collider.GetComponentInParent<PlaceView>();
                 if (pv != null)
                     return pv;
            }

            return pv;
        }
    }
}
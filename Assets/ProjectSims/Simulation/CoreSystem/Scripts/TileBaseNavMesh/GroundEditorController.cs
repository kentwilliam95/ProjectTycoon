using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.GroundEditor
{
    public class GroundEditorController : MonoBehaviour
    {
        private LayerMask _layerMaskGround;
        private RaycastHit[] _hitResult;
        private Plane raycastPlane;

        [SerializeField] private Camera _camera;
        [SerializeField] private Transform debugObj;

        private void Awake()
        {
            _layerMaskGround = LayerMask.GetMask("Ground");
            _hitResult = new RaycastHit[4];
            raycastPlane = new Plane(Vector3.down, 0.85f);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                int hitCount = Physics.RaycastNonAlloc(ray, _hitResult, 18, _layerMaskGround);
                Debug.DrawRay(ray.origin, ray.direction * 18, Color.red, 1);

                var closest = GetRaycastHitClosestToCamera(hitCount, _camera);
            
                if (closest.Item2)
                {
                    var box = closest.Item1.collider.GetComponentInParent<GroundBox>();
                    debugObj.position = box.TopCenter;
                }   
            }
        }

        private (RaycastHit, bool) GetRaycastHitClosestToCamera(int hitCount, Camera camera)
        {
            float max = float.MaxValue;
            RaycastHit target = new RaycastHit();

            for (int i = 0; i < hitCount; i++)
            {
                var dist = (_hitResult[i].transform.position - camera.transform.position).magnitude;
                if (dist < max)
                {
                    max = dist;
                    target = _hitResult[i];
                }
            }

            return (target, max != float.MaxValue ? true : false);
        }
    }
}
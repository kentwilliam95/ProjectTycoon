using ProjectSims.Simulation.CoreSystem;
using UnityEngine;

namespace Simulation.GroundEditor
{
    public class GroundEditorController : MonoBehaviour
    {
        private LayerMask _layerMaskGround;
        private RaycastHit[] _hitResult;

        [SerializeField] private Camera _camera;
        [SerializeField] private Transform debugObj;
        [SerializeField] private GroundArea _groundArea;
        
        private GroundBox _selectedGroundBox;
        private void Awake()
        {
            _layerMaskGround = LayerMask.GetMask("Ground");
            _hitResult = new RaycastHit[4];
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
                    _selectedGroundBox = box;
                }   
            }
        }

        public void ChangeToPavement()
        {
            if (!_selectedGroundBox)
            {
                Debug.Log("[GroundEditor Controller] No selected GroundBox!");
                return;
            }

            debugObj.transform.position = Vector3.one * 5000;
            _groundArea.SwapToPavementBox(_selectedGroundBox, GroundArea.GroundType.Pavement);
            _selectedGroundBox = null;
        }

        public void ChangeToGrass()
        {
            if (!_selectedGroundBox)
            {
                Debug.Log("[GroundEditor Controller] No selected GroundBox!");
                return;
            }
            
            debugObj.transform.position = Vector3.one * 5000;
            _groundArea.SwapToPavementBox(_selectedGroundBox, GroundArea.GroundType.Grass);
            _selectedGroundBox = null;
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

            return (target, max != float.MaxValue);
        }
    }
}
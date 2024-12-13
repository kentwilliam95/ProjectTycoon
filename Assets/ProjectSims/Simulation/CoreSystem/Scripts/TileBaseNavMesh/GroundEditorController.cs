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
        [SerializeField] private UIInputController _uiInputController;

        private GroundBox _selectedGroundBox;
        private Vector3 _initCamPos;

        private void Awake()
        {
            _layerMaskGround = LayerMask.GetMask("Ground");
            _hitResult = new RaycastHit[4];
            _uiInputController.OnUpdate = Input_OnDrag;
            _uiInputController.OnClick = Input_OnClick;
            _uiInputController.OnPointerRelease = Input_OnPointerRelease;
            _initCamPos = _camera.transform.position;
        }

        private void Input_OnPointerRelease()
        {
            _initCamPos = _camera.transform.position;
        }

        private void Input_OnClick()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            int hitCount = Physics.RaycastNonAlloc(ray, _hitResult, 30, _layerMaskGround);
            Debug.DrawRay(ray.origin, ray.direction * 30, Color.red, 1);
            
            var closest = GetRaycastHitClosestToCamera(hitCount, _camera);
            if (closest.Item2)
            {
                var box = closest.Item1.collider.GetComponentInParent<GroundBox>();
                debugObj.position = box.TopCenter;
                _selectedGroundBox = box;
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

        private void Input_OnDrag(Vector3 direction)
        {
            // var rightDir = _camera.transform.right* direction.x * 5;
            // var upDir = _camera.transform.up * direction.y * 5;

            // _camera.transform.position = _initCamPos + rightDir + upDir;
            
            var rightDir = _camera.transform.right* direction.x;
            var upDir = _camera.transform.up * direction.y;
            var comb = rightDir + upDir;
            _camera.transform.position += comb * Time.deltaTime * 2f;
        }
    }
}
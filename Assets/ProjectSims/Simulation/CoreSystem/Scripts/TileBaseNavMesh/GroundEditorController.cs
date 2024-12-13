using System;
using System.Collections.Generic;
using ProjectSims.Simulation.CoreSystem;
using UnityEngine;

namespace Simulation.GroundEditor
{
    public class GroundEditorController : MonoBehaviour
    {
        private enum SelectionMode
        {
            None,
            Single,
            Mix
        }

        private LayerMask _layerMaskGround;
        private RaycastHit[] _hitResult;
        private SelectionMode _selectionMode;

        [SerializeField] private Camera _camera;
        [SerializeField] private GroundArea _groundArea;
        [SerializeField] private UIInputController _uiInputController;

        [Header("Camera Speed")] [SerializeField]
        private float _camSpeed = 5;
        
        private GroundBox _selectedGroundBox;
        private List<GroundBox> _multipleSelectGroundBox;

        private void Awake()
        {
            _layerMaskGround = LayerMask.GetMask("Ground");
            _hitResult = new RaycastHit[32];
            _multipleSelectGroundBox = new List<GroundBox>();
            
            _uiInputController.OnUpdate = Input_OnDrag;
            _uiInputController.OnClick = Input_OnClick;
            _uiInputController.OnPointerRelease = Input_OnPointerRelease;
        }

        private void Start()
        {
            _groundArea.LoadGround();
        }

        private void Input_OnPointerRelease()
        {
            for (int i = _multipleSelectGroundBox.Count - 1; i >= 0 ; i--)
            {
                var box = _multipleSelectGroundBox[i];
                if (box.EditState == GroundBox.MarkState.PrepareToDelete)
                {
                    box.SetState(GroundBox.MarkState.ReadyToDelete);
                }

                if (box.EditState == GroundBox.MarkState.Delete)
                {
                    box.SetState(GroundBox.MarkState.None);
                    _multipleSelectGroundBox.RemoveAt(i);
                }
            }   
        }

        private void Input_OnClick()
        {
            // HandleSingleSelect();
        }
        
        private void HandleSingleSelect()
        {
            if (_selectionMode != SelectionMode.Single) { return; }

            GroundBox box = GetRaycastMousePos<GroundBox>(Input.mousePosition, _layerMaskGround);
            if (!box) { return; }

            if (box.EditState == GroundBox.MarkState.None)
            {
                if (!_multipleSelectGroundBox.Contains(box))
                {
                    box.SetState( GroundBox.MarkState.PrepareToDelete);
                    box.Select();
                    _multipleSelectGroundBox.Add(box);
                }   
            }
            
            if(box.EditState == GroundBox.MarkState.ReadyToDelete)
            {
                box.SetState(GroundBox.MarkState.Delete);
                box.UnSelect();
            }
        }

        private T GetRaycastMousePos<T>(Vector3 pos, LayerMask layerMask)
        {
            T res = default;
            Ray ray = _camera.ScreenPointToRay(pos);
            int hitCount = Physics.RaycastNonAlloc(ray, _hitResult, 30, layerMask);
            var closest = GetRaycastHitClosestToCamera(hitCount, _camera);
            if (closest.Item2)
            {
                var obj = closest.Item1.collider.GetComponentInParent<T>();
                if (obj == null) { return res; }

                res = obj;
            }

            return res;
        }

        public void ChangeToPavement()
        {
            if (_multipleSelectGroundBox.Count == 0)
            {
                Debug.Log("[GroundEditor Controller] No selected GroundBox!");
                return;
            }
            
            for (int i = 0; i < _multipleSelectGroundBox.Count; i++)
            {
                _groundArea.SwapToPavementBox(_multipleSelectGroundBox[i], GroundArea.GroundType.Pavement);
            }
            ResetBoxColor();
        }

        public void ChangeToGrass()
        {
            if (_multipleSelectGroundBox.Count == 0)
            {
                Debug.Log("[GroundEditor Controller] No selected GroundBox!");
                return;
            }
            
            for (int i = 0; i < _multipleSelectGroundBox.Count; i++)
            {
                _groundArea.SwapToPavementBox(_multipleSelectGroundBox[i], GroundArea.GroundType.Grass);
            }
            ResetBoxColor();
        }

        public void ClearGround()
        {
            _groundArea.ClearGround();
        }

        public void BakeNavMesh()
        {
            _groundArea.BakeNavMesh();
        }

        public void SelectionSingle()
        {
            ResetBoxColor();
            if (_selectionMode == SelectionMode.Single)
            {
                _selectionMode = SelectionMode.None;
                return;
            }
            _selectionMode = SelectionMode.Single;
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
            HandleSingleSelect();
            HandleCameraMovement(direction);
        }

        private void HandleCameraMovement(Vector3 direction)
        {
            if (_selectionMode == SelectionMode.Single) { return; }

            var rightDir = _camera.transform.right* direction.x;
            var upDir = _camera.transform.up * direction.y;
            var comb = rightDir + upDir;
            _camera.transform.position += comb * Time.deltaTime * _camSpeed;
        }

        private void ResetBoxColor()
        {
            for (int i = 0; i < _multipleSelectGroundBox.Count; i++) { _multipleSelectGroundBox[i].UnSelect(); }
            _multipleSelectGroundBox.Clear();

            if (_selectedGroundBox)
            {
                _selectedGroundBox.UnSelect();   
            }
            
            _selectedGroundBox = null;
        }
    }
}
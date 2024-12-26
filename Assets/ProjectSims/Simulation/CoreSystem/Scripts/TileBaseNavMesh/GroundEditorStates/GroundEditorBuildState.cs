using System.Collections;
using ProjectSims.Simulation.Scripts.StateMachine;
using Simulation.GroundEditor;
using Simulation.UI;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectSims.Simulation.GroundEditorStates
{
    public class GroundEditorBuildState : IState<GroundEditorController>
    {
        private enum TransformType
        {
            None,
            Move,
            Rotate
        }

        private TransformType _editType;
        private GroundEditorController _controller;
        private RaycastHit[] _hitResult = new RaycastHit[8];
        private Vector3 _initPoint;
        private Decoration _selectedGo;

        public void OnEnter(GroundEditorController t)
        {
            _controller = t;
            _controller.UiInputController.OnUpdate = HandleInput_OnUpdate;

            var ui = _controller.UIGroundEditorBuild;
            ui.ButtonCheck.interactable = false;
            ui.ButtonDelete.interactable = false;
            ui.ButtonMove.interactable = false;
            ui.ButtonRotate.interactable = false;

            _controller.UIGroundEditorBuild.OnButtonDoneClicked = ExitBuildMode;
            _controller.UIGroundEditorBuild.OnItemClicked = Handle_ItemOnClicked;
            _controller.UIGroundEditorBuild.OnButtonRotateClicked = EnterRotateMode;
            _controller.UIGroundEditorBuild.OnButtonMoveClicked = EnterMoveMode;
            _controller.UIGroundEditorBuild.OnButtonCheckClicked = HandleButtonCheck_OnClicked;
            _controller.UIGroundEditorBuild.OnButtonDeleteClicked = EnterDeleteMode;
            
            _controller.UiInputController.OnScrolling = Input_OnScrolled;
            _controller.UiInputController.OnPinch = Input_OnPinched;
            _controller.UiInputController.OnPointerRelease = Input_OnRelease;

            _controller.UiInputController.OnClick = HandleInput_OnClicked;

            _controller.UIGroundEditorBuild.Show();
            _controller.UIGroundEditorBuild.Init();

            _controller.UIGroundEditorEdit.Hide();
            _controller.PopupGroundFileEditor.Hide();

            _selectedGo = null;
            _initPoint = Vector3.zero;
            _controller.UIGroundEditorBuild.Title.SetText("Select or build object!");
        }

        public void OnUpdate(GroundEditorController t) { }

        public void OnExit(GroundEditorController t)
        {
            _controller.UIGroundEditorBuild.OnButtonRotateClicked = null;
            _controller.UIGroundEditorBuild.OnButtonMoveClicked = null;
            _controller.UIGroundEditorBuild.OnButtonCheckClicked = null;
            _controller.UIGroundEditorBuild.OnButtonDeleteClicked = null;
            _controller.UIGroundEditorBuild.OnButtonDoneClicked = null;
            _controller.UIGroundEditorBuild.OnItemClicked = null;
            _controller.UiInputController.OnUpdate = null;
            _controller.UiInputController.OnClick = null;
            
            _controller.UiInputController.OnScrolling = null;
            _controller.UiInputController.OnPinch = null;
            _controller.UiInputController.OnPointerRelease = null;

            _controller.UIGroundEditorBuild.Hide();
            _selectedGo = null;
            _initPoint = Vector3.zero;
        }

        private void ExitBuildMode()
        {
            _controller.SaveDecorations();
            _controller.ChangeState(new GroundEditorNormalState());
            _controller.ShowLoadingScreenFor(0.5f, "Saving Decorations!");
        }

        private void HandleInput_OnClicked(Vector3 pos)
        {
            if (_selectedGo != null)
            {
                bool isValid = CheckIfDecorCollide(_selectedGo, _selectedGo.transform.position);
                if (isValid) { return; }
            }

            var decor = _controller.GetRaycastMousePos<Decoration>(pos, LayerMask.GetMask("Decoration"));
            if (decor != null)
            {
                decor.Select();
                EnterMoveMode();
                _selectedGo = decor;
                _initPoint = _selectedGo.transform.position;
                _controller.mainCamera.MoveToTarget(decor.transform.position);
                UpdateButtonsInteractable(true);
            }
        }

        private void HandleInput_OnUpdate(Vector3 dir)
        {
            if (_editType == TransformType.None)
            {
                _controller.MoveCameraByDragging(dir, _controller.CamSpeed);
            }

            if (_selectedGo == null)
            {
                return;
            }

            if (_editType == TransformType.Move)
            {
                _controller.UIGroundEditorBuild.Title.SetText("Drag Selected object with your finger");
                dir.z = dir.y;
                dir.y = 0;

                var angleaxis = Quaternion.AngleAxis(45, Vector3.up) * dir;
                var nextPos = _initPoint + angleaxis * Time.deltaTime * _controller.MoveObjectSpeed;
                
                if (!_controller.GroundArea.IsPointInsideBoundary(nextPos)) { return; }
                UpdateUIButtonDroppableObject(nextPos);
                
                _initPoint += angleaxis * Time.deltaTime * _controller.MoveObjectSpeed;
                _selectedGo.transform.position = _initPoint;
                
                dir.y = dir.z;
                dir.z = 0;
                _controller.MoveCameraByDragging(dir, _controller.MoveObjectSpeed);
            }

            if (_editType == TransformType.Rotate)
            {
                dir.z = dir.y;
                dir.y = 0;

                var angleaxis = Quaternion.AngleAxis(45, Vector3.up) * dir;
                _selectedGo.transform.forward = angleaxis;
            }
        }

        private void Handle_ItemOnClicked(DecorationSO.AssetDetail so)
        {
            if (_selectedGo != null)
            {
                return;
            }

            EnterMoveMode();
            var point = GetPoint(_controller.UiInputController.Center);
            point.y = 0.5f;
            _initPoint = point;
            _selectedGo = _controller.GroundArea.SpawnDecoration<Decoration>(so.Template, point, quaternion.identity);
            _selectedGo.InitSpawnAnimation();
            
            UpdateUIButtonDroppableObject(point);
        }

        private Vector3 GetPoint(Vector3 pos)
        {
            var ray = _controller.mainCamera.ScreenPointToRay(pos);
            int hitCount = Physics.RaycastNonAlloc(ray, _hitResult, 30, GroundEditorController.LayerMaskGround);
            if (hitCount > 0)
            {
                return _hitResult[0].point;
            }

            return Vector3.zero;
        }

        private void EnterRotateMode()
        {
            _controller.UIGroundEditorBuild.Title.SetText("Rotate the object by dragging your finger!");
            _editType = TransformType.Rotate;

            UpdateButtonsInteractable(true);
        }

        private void EnterMoveMode()
        {
            _controller.UIGroundEditorBuild.Title.SetText("Move current object by dragging your finger!");
            _editType = TransformType.Move;

            UpdateButtonsInteractable(true);
        }

        private void EnterDeleteMode()
        {
            if (!_selectedGo)
            {
                return;
            }

            _controller.DestroyGameObject(_selectedGo.gameObject);
            _selectedGo = null;
            _editType = TransformType.None;

            UpdateButtonsInteractable(false);
            _controller.UIGroundEditorBuild.Title.SetText("Object deleted!");
        }

        private void HandleButtonCheck_OnClicked()
        {
            _controller.UIGroundEditorBuild.Title.SetText("Select or build object!");
            _editType = TransformType.None;
            _selectedGo = null;

            UpdateButtonsInteractable(false);
        }

        private void UpdateButtonsInteractable(bool isInteractable)
        {
            var ui = _controller.UIGroundEditorBuild;
            ui.ButtonCheck.interactable = isInteractable;
            ui.ButtonDelete.interactable = isInteractable;
            ui.ButtonMove.interactable = isInteractable;
            ui.ButtonRotate.interactable = isInteractable;
        }

        private bool CheckIfDecorCollide(Decoration target, Vector3 position)
        {
            int hitCount = Physics.BoxCastNonAlloc(position, target._extends, Vector3.down, _hitResult, quaternion.identity, target._length, GroundEditorController.LayerMaskDecoration);
            for (int i = 0; i < hitCount; i++)
            {
                var decor = _hitResult[i].collider.GetComponentInParent<Decoration>();
                if (decor != null && decor != target)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateUIButtonDroppableObject(Vector3 position)
        {
            bool isCollide = CheckIfDecorCollide(_selectedGo, position);
            _controller.UIGroundEditorBuild.ButtonCheck.interactable = !isCollide;
            _controller.UIGroundEditorBuild.ButtonDone.interactable = !isCollide;
            
            if (isCollide)
            {
                _controller.UIGroundEditorBuild.Title.SetText("Unable to drop here!");   
            }
        }
        
        private void Input_OnScrolled(Vector2 delta)
        {
            _controller.SetCameraZoom(delta.y);
        }

        private void Input_OnPinched(float val)
        {
            _controller.SetCameraZoomByPinch(val);
        }
        
        private void Input_OnRelease()
        {
            _controller.SetZoomLevel();
        }
    }
}
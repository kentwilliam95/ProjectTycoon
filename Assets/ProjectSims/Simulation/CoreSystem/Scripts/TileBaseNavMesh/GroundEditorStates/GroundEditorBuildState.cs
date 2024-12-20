using ProjectSims.Simulation.Scripts.StateMachine;
using Simulation.GroundEditor;
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
        private GameObject _selectedGo;

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
            
            _controller.UIGroundEditorBuild.Show();
            _controller.UIGroundEditorBuild.Init();

            _controller.UIGroundEditorEdit.Hide();
            _controller.PopupGroundFileEditor.Hide();
            
            _selectedGo = null;
            _initPoint = Vector3.zero;
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
            _controller.UIGroundEditorBuild.Hide();
            _selectedGo = null;
            _initPoint = Vector3.zero;
            Debug.Log("Exit");
        }

        private void ExitBuildMode()
        {
            _controller.ChangeState(new GroundEditorNormalState());
        }

        private void HandleInput_OnUpdate(Vector3 dir)
        {
            if (_selectedGo == null) { return; }

            if (_editType == TransformType.Move)
            {
                dir.z = dir.y;
                dir.y = 0;

                var angleaxis = Quaternion.AngleAxis(45, Vector3.up) * dir;
                if (!_controller.GroundArea.IsPointInsideBoundary(_initPoint + angleaxis * Time.deltaTime))
                {
                    return;
                }

                _initPoint += angleaxis * Time.deltaTime;
                _selectedGo.transform.position = _initPoint;

                dir.y = dir.z;
                dir.z = 0;
                _controller.MoveCameraByDragging(dir, 1f);
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
            if (_selectedGo != null) { return;}

            EnterMoveMode();
            var point = GetPoint(_controller.UiInputController.Center);
            point.y = 0.5f;
            _initPoint = point;
            _selectedGo = GameObject.Instantiate<GameObject>(so.Template, point, quaternion.identity);
        }

        private Vector3 GetPoint(Vector3 pos)
        {
            var ray = _controller.Camera.ScreenPointToRay(pos);
            Debug.DrawRay(ray.origin, ray.direction * 30, Color.cyan);
            int hitCount = Physics.RaycastNonAlloc(ray, _hitResult, 30, _controller.LayerMaskGround);
            if (hitCount > 0)
            {
                return _hitResult[0].point;
            }

            return Vector3.zero;
        }

        private void EnterRotateMode()
        {
            _controller.UIGroundEditorBuild.Title.SetText("Rotate Mode");
            _editType = TransformType.Rotate;
            
            var ui = _controller.UIGroundEditorBuild;
            ui.ButtonCheck.interactable = true;
            ui.ButtonDelete.interactable = true;
            ui.ButtonMove.interactable = true;
            ui.ButtonRotate.interactable = true;
        }

        private void EnterMoveMode()
        {
            _controller.UIGroundEditorBuild.Title.SetText("Move Mode");
            _editType = TransformType.Move;
            
            var ui = _controller.UIGroundEditorBuild;
            ui.ButtonCheck.interactable = true;
            ui.ButtonDelete.interactable = true;
            ui.ButtonMove.interactable = true;
            ui.ButtonRotate.interactable = true;
        }

        private void EnterDeleteMode()
        {
            if (!_selectedGo) { return;}
            GameObject.Destroy(_selectedGo);
            _selectedGo = null;
            _editType = TransformType.None;
            
            var ui = _controller.UIGroundEditorBuild;
            ui.ButtonCheck.interactable = false;
            ui.ButtonDelete.interactable = false;
            ui.ButtonMove.interactable = false;
            ui.ButtonRotate.interactable = false;
        }

        private void HandleButtonCheck_OnClicked()
        {
            _controller.UIGroundEditorBuild.Title.SetText(string.Empty);
            _editType = TransformType.None;
            _selectedGo = null;
            
            var ui = _controller.UIGroundEditorBuild;
            ui.ButtonCheck.interactable = false;
            ui.ButtonDelete.interactable = false;
            ui.ButtonMove.interactable = false;
            ui.ButtonRotate.interactable = false;
        }
    }
}
using ProjectSims.Simulation.Scripts.StateMachine;
using Simulation.GroundEditor;
using UnityEngine;

namespace ProjectSims.Simulation.GroundEditorStates
{
    public class GroundEditorNormalState:IState<GroundEditorController>
    {
        private GroundEditorController _controller;
        
        public void OnEnter(GroundEditorController t)
        {
            _controller = t;
            
            _controller.Menu.Show();
            _controller.UIGroundEditorEdit.Hide();
            _controller.UIGroundEditorBuild.Hide();
            
            _controller.UiInputController.OnClick = Input_OnClick;
            _controller.UiInputController.OnUpdate = Input_OnDragging;
            
            _controller.UiInputController.OnScrolling = Input_OnScrolled;
            _controller.UiInputController.OnPinch = Input_OnPinched;
            _controller.UiInputController.OnPointerRelease = Input_OnRelease;
            
            t.Menu.ButtonEditModeOnClicked = EnterEditMode;
            t.Menu.ButtonBuildModeOnClicked = EnterBuildMode;
        }

        public void OnUpdate(GroundEditorController t)
        {
            
        }

        public void OnExit(GroundEditorController t)
        {
            _controller.Menu.Hide();
            
            t.Menu.ButtonEditModeOnClicked = null;
            t.Menu.ButtonBuildModeOnClicked = null;
            
            _controller.UiInputController.OnClick = null;
            _controller.UiInputController.OnUpdate = null;
            
            _controller.UiInputController.OnScrolling = null;
            _controller.UiInputController.OnPinch = null;
            _controller.UiInputController.OnPointerRelease = null;
            
        }
        private void EnterEditMode()
        {
            _controller.ChangeState(new GroundEditorEditState());
        }

        private void EnterBuildMode()
        {
            _controller.ChangeState(new GroundEditorBuildState());
        }

        private void Input_OnDragging(Vector3 direction)
        {
            _controller.MoveCameraByDragging(direction, _controller.CamSpeed);
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

        private void Input_OnClick(Vector3 mousePos)
        {
            GroundBox box = _controller.GetRaycastMousePos<GroundBox>(mousePos,GroundEditorController.LayerMaskGround);
            _controller.MoveAgentsTo(box);
        }
    }
}
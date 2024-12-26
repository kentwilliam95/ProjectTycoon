using ProjectSims.Simulation.Scripts.StateMachine;
using Simulation.GroundEditor;
using UnityEngine;

namespace ProjectSims.Simulation.GroundEditorStates
{
    public class GroundEditorNormalState:IState<GroundEditorController>
    {
        private GroundEditorController _controller;
        private float _zoomLevel = 6.5f;
        
        public void OnEnter(GroundEditorController t)
        {
            _controller = t;
            
            _controller.Menu.Show();
            _controller.UIGroundEditorEdit.Hide();
            _controller.UiInputController.OnUpdate = Input_OnDragging;

            _controller.UiInputController.OnScrolling = Input_OnScrolled;
            _controller.UiInputController.OnPinch = Input_OnPinched;

            _controller.UiInputController.OnClick = Input_OnClick;
            _controller.UiInputController.OnPointerRelease = Input_OnRelease;
            
            t.Menu.ButtonEditModeOnClicked = EnterEditMode;
            _zoomLevel = _controller.Camera.orthographicSize;
        }

        public void OnUpdate(GroundEditorController t)
        {
            
        }

        public void OnExit(GroundEditorController t)
        {
            t.Menu.ButtonEditModeOnClicked = null;
        }
        private void EnterEditMode()
        {
            _controller.ChangeState(new GroundEditorEditState());
        }

        private void Input_OnDragging(Vector3 direction)
        {
            _controller.MoveCameraByDragging(direction);
        }

        private void Input_OnScrolled(Vector2 delta)
        {
            float yaxis = delta.y;
            yaxis = yaxis * 8f;
            
            var cam = _controller.Camera;
            var size = cam.orthographicSize;

            var zoom = size + yaxis * Time.deltaTime;
            zoom = Mathf.Clamp(zoom, 2, 10);
            cam.orthographicSize = zoom;
        }

        private void Input_OnPinched(float val)
        {
            var cam = _controller.Camera;
            cam.orthographicSize = Mathf.Clamp( _zoomLevel - val,2, 10);
        }

        private void Input_OnClick(Vector3 mousePos)
        {
            GroundBox box = _controller.GetRaycastMousePos<GroundBox>(mousePos,_controller.LayerMaskGround);
            _controller.MoveAgentsTo(box);
        }

        private void Input_OnRelease()
        {
            _zoomLevel = _controller.Camera.orthographicSize;
        }
    }
}
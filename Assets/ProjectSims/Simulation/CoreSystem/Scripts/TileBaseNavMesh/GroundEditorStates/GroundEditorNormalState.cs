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
            
            _controller.UiInputController.OnUpdate = Input_OnDragging;
            
            _controller.UiInputController.OnClick = Input_OnClick;
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

        private void Input_OnClick(Vector3 mousePos)
        {
            GroundBox box = _controller.GetRaycastMousePos<GroundBox>(mousePos,_controller.LayerMaskGround);
            _controller.MoveAgentsTo(box);
        }
    }
}
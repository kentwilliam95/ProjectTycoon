using System;
using System.Collections.Generic;
using ProjectSims.Simulation.CoreSystem;
using ProjectSims.Simulation.Scripts.StateMachine;
using Simulation.GroundEditor;
using UnityEngine;

namespace ProjectSims.Simulation.GroundEditorStates
{
    public class GroundEditorEditState : IState<GroundEditorController>
    {
        private enum SelectionMode
        {
            None,
            Single,
        }

        private GroundEditorController _controller;
        private List<GroundBox> _multipleSelectGroundBox;
        private SelectionMode _selectionMode;
        private float _orthoSize;
        private int _bakeCount;
        private int _editCount;

        public void OnEnter(GroundEditorController t)
        {
            _multipleSelectGroundBox = new List<GroundBox>();

            t.UiInputController.OnUpdate = Input_OnDrag;
            t.UiInputController.OnClick = Input_OnClick;
            t.UiInputController.OnPointerRelease = Input_OnPointerRelease;

            t.ButtonNewFileEditor.onClick.AddListener(OpenFileEditor);
            t.PopupGroundFileEditor.OnButtonSaveClicked = FileEditor_OnSaveClicked;

            t.UIGroundEditorEdit.DisableControls();
            t.UIGroundEditorEdit.EnableMenu();

            t.UIGroundEditorEdit.OnZoomChange = Zoom_OnValueChanged;
            _orthoSize = t.Camera.orthographicSize;
            _controller = t;

            t.UIGroundEditorEdit.EnableSelection();
            t.UIGroundEditorEdit.Show();
            t.Menu.Hide();

            t.UIGroundEditorEdit.SetTitle("Edit Mode!");
            t.UIGroundEditorEdit.OnButtonEditClicked = HandleButtonEditClicked;
            t.UIGroundEditorEdit.OnButtonDoneClicked = HandleButtonDoneClicked;
            t.UIGroundEditorEdit.OnButtonGrassClicked = HandleChangeToGrass;
            t.UIGroundEditorEdit.OnButtonPavementClicked = HandleToPavement;
            t.UIGroundEditorEdit.OnButtonLoadClicked = FileEditor_OnLoadClicked;
        }

        private void HandleButtonNewClicked() { }

        public void OnUpdate(GroundEditorController t) { }

        private void HandleButtonEditClicked()
        {
            if (_selectionMode == SelectionMode.Single)
            {
                ExitEditMode();
            }
            else
            {
                EnterEditMode();
            }
        }

        private void HandleButtonDoneClicked()
        {
            _controller.GroundArea.SaveGround();

            if (_bakeCount <= 0 && _editCount > 0)
            {
                _controller.BakeNavMesh();
            }

            _controller.ChangeState(new GroundEditorNormalState());
        }

        public void OnExit(GroundEditorController t)
        {
            t.UiInputController.OnUpdate = null;
            t.UiInputController.OnClick = null;
            t.UiInputController.OnPointerRelease = null;
            t.ButtonNewFileEditor.onClick.RemoveListener(OpenFileEditor);
            t.PopupGroundFileEditor.OnButtonSaveClicked = null;
            t.UIGroundEditorEdit.OnZoomChange = null;

            t.UIGroundEditorEdit.DisableControls();
            t.UIGroundEditorEdit.EnableMenu();
            t.UIGroundEditorEdit.EnableSelection();
            t.UIGroundEditorEdit.UnHighlightButtonReplace();

            ResetBoxColor();
        }

        private void Input_OnClick(Vector3 mousePos) { }

        private void Input_OnDrag(Vector3 direction)
        {
            HandleSingleSelect_OnDrag();
            HandleCameraMovement_OnDrag(direction);
        }

        private void Input_OnPointerRelease()
        {
            HandleEditMode();
        }

        private void HandleSingleSelect_OnDrag()
        {
            if (_selectionMode != SelectionMode.Single)
            {
                return;
            }

            _controller.UIGroundEditorEdit.SetTitle(_multipleSelectGroundBox.Count > 0
                ? "Selected could be converted to grass or pavement, see Control UI"
                : "select ground to continue.");

            GroundBox box = _controller.GetRaycastMousePos<GroundBox>(Input.mousePosition, _controller.LayerMaskGround);
            if (!box)
            {
                return;
            }

            if (box.EditState == GroundBox.MarkState.None)
            {
                if (!_multipleSelectGroundBox.Contains(box))
                {
                    box.SetState(GroundBox.MarkState.PrepareToDelete);
                    box.Select();
                    _multipleSelectGroundBox.Add(box);
                }
            }

            if (box.EditState == GroundBox.MarkState.ReadyToDelete)
            {
                box.SetState(GroundBox.MarkState.Delete);
                box.UnSelect();
            }
        }

        private void HandleEditMode()
        {
            if (_selectionMode != SelectionMode.Single)
            {
                return;
            }

            for (int i = _multipleSelectGroundBox.Count - 1; i >= 0; i--)
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

            _controller.UIGroundEditorEdit.SetTitle(_multipleSelectGroundBox.Count > 0
                ? "Selected could be converted to grass or pavement, see Control UI"
                : "select ground to continue.");
        }

        private void HandleCameraMovement_OnDrag(Vector3 direction)
        {
            if (_selectionMode == SelectionMode.Single)
            {
                return;
            }

            _controller.MoveCameraByDragging(direction);
        }

        private void Zoom_OnValueChanged(float value)
        {
            _controller.Camera.orthographicSize = _orthoSize + value;
        }

        private void HandleToPavement()
        {
            if (_multipleSelectGroundBox.Count == 0)
            {
                Debug.Log("[GroundEditor Controller] No selected GroundBox!");
                return;
            }

            for (int i = 0; i < _multipleSelectGroundBox.Count; i++)
            {
                _controller.GroundArea.SwapToPavementBox(_multipleSelectGroundBox[i], GroundArea.GroundType.Pavement);
            }

            ResetBoxColor();
        }

        private void HandleChangeToGrass()
        {
            if (_multipleSelectGroundBox.Count == 0)
            {
                Debug.Log("[GroundEditor Controller] No selected GroundBox!");
                return;
            }

            for (int i = 0; i < _multipleSelectGroundBox.Count; i++)
            {
                _controller.GroundArea.SwapToPavementBox(_multipleSelectGroundBox[i], GroundArea.GroundType.Grass);
            }

            ResetBoxColor();
        }

        private void ExitEditMode()
        {
            ResetBoxColor();

            _selectionMode = SelectionMode.None;

            _controller.UIGroundEditorEdit.DisableControls();
            _controller.UIGroundEditorEdit.EnableMenu();
            _controller.UIGroundEditorEdit.UnHighlightButtonReplace();
            _controller.GroundArea.SaveGround();

            _controller.BakeNavMesh();
            _bakeCount++;
        }

        private void EnterEditMode()
        {
            _editCount++;
            _selectionMode = SelectionMode.Single;
            _controller.UIGroundEditorEdit.SetTitle("Select ground and replace it with pavement or grass!");
            _controller.UIGroundEditorEdit.EnableControls();
            _controller.UIGroundEditorEdit.DisableMenu();
            _controller.UIGroundEditorEdit.HighlightButtonReplace();
        }

        private void ResetBoxColor()
        {
            for (int i = 0; i < _multipleSelectGroundBox.Count; i++)
            {
                _multipleSelectGroundBox[i].ResetState();
            }

            _multipleSelectGroundBox.Clear();
        }

        private void FileEditor_OnLoadClicked()
        {
            _controller.GroundArea.LoadGround();
        }

        private void FileEditor_OnSaveClicked(int x, int y)
        {
            x = Mathf.Min(x, 10);
            y = Mathf.Min(y, 10);

            _controller.GroundArea.GenerateDefaultGround(x, y);
            _controller.GroundArea.LoadGround();
        }

        private void OpenFileEditor()
        {
            _controller.PopupGroundFileEditor.Show();
        }
    }
}
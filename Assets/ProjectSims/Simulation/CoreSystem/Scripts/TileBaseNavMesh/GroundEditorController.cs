using System;
using System.Collections;
using System.Collections.Generic;
using ProjectSims.Simulation.CoreSystem;
using Simulation.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        [SerializeField] private UIGroundEditor _uiGroundEditor;

        [FormerlySerializedAs("_groundEditorFile")] [Header("UI File Editor")] [SerializeField]
        private UIGroundEditorFile _groundFileEditor;

        [SerializeField] private Button _buttonNewFileEditor;

        [Header("Camera Speed")] [SerializeField]
        private float _camSpeed = 5;

        private float _orthoSize;

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

            _buttonNewFileEditor.onClick.AddListener(OpenFileEditor);
            _groundFileEditor.OnButtonSaveClicked = FileEditor_OnSaveClicked;

            _uiGroundEditor.DisableControls();
            _uiGroundEditor.EnableMenu();
            _uiGroundEditor.DisableSelection();

            _uiGroundEditor.OnZoomChange = Zoom_OnValueChanged;
            _orthoSize = _camera.orthographicSize;
        }

        private void Start()
        {
            UILoading.Instance.Hide();
            _groundArea.LoadGround();
            _uiGroundEditor.EnableSelection();
            _uiGroundEditor.Show();
            _groundFileEditor.Hide();
        }


        #region Input Handle

        private void Input_OnDrag(Vector3 direction)
        {
            HandleSingleSelect();
            HandleCameraMovement(direction);
        }
        
        private void Input_OnPointerRelease()
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

            _uiGroundEditor.SetTitle(_multipleSelectGroundBox.Count > 0
                ? "Selected could be converted to grass or pavement, see Control UI"
                : "select ground to continue.");
        }

        
        private void Input_OnClick() { }

        private void HandleCameraMovement(Vector3 direction)
        {
            if (_selectionMode == SelectionMode.Single)
            {
                return;
            }

            var rightDir = _camera.transform.right * direction.x;
            var upDir = _camera.transform.up * direction.y;
            var comb = rightDir + upDir;
            _camera.transform.position += comb * Time.deltaTime * _camSpeed;
        }
        
        private void HandleSingleSelect()
        {
            if (_selectionMode != SelectionMode.Single)
            {
                return;
            }

            _uiGroundEditor.SetTitle(_multipleSelectGroundBox.Count > 0
                ? "Selected could be converted to grass or pavement, see Control UI"
                : "select ground to continue.");

            GroundBox box = GetRaycastMousePos<GroundBox>(Input.mousePosition, _layerMaskGround);
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
        
        private T GetRaycastMousePos<T>(Vector3 pos, LayerMask layerMask)
        {
            T res = default;
            Ray ray = _camera.ScreenPointToRay(pos);
            int hitCount = Physics.RaycastNonAlloc(ray, _hitResult, 30, layerMask);
            var closest = GetRaycastHitClosestToCamera(hitCount, _camera);
            if (closest.Item2)
            {
                var obj = closest.Item1.collider.GetComponentInParent<T>();
                if (obj == null)
                {
                    return res;
                }

                res = obj;
            }

            return res;
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
        
        #endregion

        #region File Editor
        
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
                _uiGroundEditor.DisableControls();
                _uiGroundEditor.EnableMenu();
                _uiGroundEditor.UnHighlightButtonReplace();
                _uiGroundEditor.SetTitle(string.Empty);
                StartCoroutine(StartBakeNavMesh());
                return;
            }

            _uiGroundEditor.SetTitle("Select ground and replace it with pavement or grass!");
            _uiGroundEditor.EnableControls();
            _uiGroundEditor.DisableMenu();
            _uiGroundEditor.HighlightButtonReplace();
            _selectionMode = SelectionMode.Single;
        }
        
        private void FileEditor_OnSaveClicked(int x, int y)
        {
            x = Mathf.Max(x, 20);
            y = Mathf.Max(y, 20);
            
            _groundArea.GenerateDefaultGround(x, y);
            _groundArea.LoadGround();
        }

        private void OpenFileEditor()
        {
            _groundFileEditor.Show();
        }

        private void Zoom_OnValueChanged(float value)
        {
            _camera.orthographicSize = _orthoSize + value;
        }

        #endregion

        private void ResetBoxColor()
        {
            for (int i = 0; i < _multipleSelectGroundBox.Count; i++)
            {
                _multipleSelectGroundBox[i].ResetState();
            }

            _multipleSelectGroundBox.Clear();

            if (_selectedGroundBox)
            {
                _selectedGroundBox.ResetState();
            }

            _selectedGroundBox = null;
        }

        private IEnumerator StartBakeNavMesh()
        {
            UILoading.Instance.Show("Generating Mesh!");
            yield return new WaitForSeconds(1f);
            _groundArea.BakeNavMesh();
            yield return new WaitForSeconds(1f);
            UILoading.Instance.Hide();
        }
    }
}
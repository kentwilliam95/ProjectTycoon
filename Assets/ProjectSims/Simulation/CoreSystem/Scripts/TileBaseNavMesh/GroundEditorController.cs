using System;
using System.Collections;
using Cinemachine;
using ProjectSims.Simulation.CoreSystem;
using ProjectSims.Simulation.CoreSystem.Scripts.Interface;
using ProjectSims.Simulation.GroundEditorStates;
using ProjectSims.Simulation.Scripts.StateMachine;
using Simulation.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Simulation.GroundEditor
{
    public class GroundEditorController : MonoBehaviour
    {
        public static LayerMask LayerMaskGround;
        public static LayerMask LayerMaskDecoration;
        
        private RaycastHit[] _hitResult;
        private Coroutine _coroutineBakeMesh;
        private IAgent[] _agents;

        private StateMachine<GroundEditorController> _stateMachine;
        
        [SerializeField] private MainCamera _mainCamera;
        public MainCamera mainCamera => _mainCamera;
        
        [SerializeField] private GroundArea _groundArea;
        public GroundArea GroundArea => _groundArea;
        
        [SerializeField] private UIInputController _uiInputController;
        public UIInputController UiInputController => _uiInputController;

        [SerializeField]
        private UIGroundEditorEdit _uiGroundEditorEdit;
        public UIGroundEditorEdit UIGroundEditorEdit => _uiGroundEditorEdit;
        
        [Header("UI File Editor")]
        [SerializeField]
        private UIPopupGroundEditorFile _popupGroundFileEditor;
        public UIPopupGroundEditorFile PopupGroundFileEditor => _popupGroundFileEditor;

        [field: SerializeField] public UIGroundEditorBuild UIGroundEditorBuild { get; private set; }

        [SerializeField] private Button _buttonNewFileEditor;
        public Button ButtonNewFileEditor => _buttonNewFileEditor;
        
        [SerializeField] private UIGroundEditorMenu _menu;
        public UIGroundEditorMenu Menu => _menu;
        
        public float OrthoSize { get; private set; }

        [Header("Camera Speed")] 
        [SerializeField]
        private float _camSpeed = 5;
        public float CamSpeed => _camSpeed;
        [field: SerializeField] public float MoveObjectSpeed { get; private set; }

        [SerializeField] private DecorationSO _decorationSo; 
        private void Awake()
        {
            _stateMachine = new StateMachine<GroundEditorController>(this);
            
            LayerMaskGround = LayerMask.GetMask("Ground");
            LayerMaskDecoration = LayerMask.GetMask("Decoration");
            
            _hitResult = new RaycastHit[32];
        }
        
        private IEnumerator Start()
        {
            UILoading.Instance.Hide();
            _decorationSo.InitDictionary();
            _popupGroundFileEditor.Hide(true);
            _uiGroundEditorEdit.Hide(true);
            UIGroundEditorBuild.Hide(true);
            _menu.Show();
            
            _agents = GetComponentsInChildren<IAgent>();
            foreach (var agent in _agents)
            {
                agent.DisableAgent();
            }
            
            yield return _groundArea.LoadGround();
            yield return _groundArea.LoadDecorations();
            
            yield return BakeNavMesh(() =>
            {
                foreach (var agent in _agents)
                {
                    agent.EnableAgent();
                }
            });
            
            _stateMachine.ChangeState(new GroundEditorNormalState());
            OrthoSize = mainCamera.Orthographic;
        }
        
        public Coroutine BakeNavMesh(Action onComplete = null)
        {
            if (_coroutineBakeMesh != null)
            {
                StopCoroutine(_coroutineBakeMesh);
            }

            _coroutineBakeMesh = StartCoroutine(StartBakeNavMesh(onComplete));
            return _coroutineBakeMesh;
        }

        public void ChangeState(IState<GroundEditorController> state)
        {
            _stateMachine.ChangeState(state);
        }

        public void MoveCameraByDragging(Vector3 direction, float speed)
        {
            var camTr = mainCamera.transform;
            var rightDir = camTr.right * direction.x;
            var upDir = camTr.up * direction.y;
            var comb = rightDir + upDir;
            camTr.transform.position += comb * (Time.deltaTime * speed);
        }

        private IEnumerator StartBakeNavMesh(Action onComplete = null)
        {
            UILoading.Instance.Show("Generating Mesh!");
            yield return new WaitForSeconds(1f);
            _groundArea.BakeNavMesh();
            yield return new WaitForSeconds(1f);
            UILoading.Instance.Hide();
            
            onComplete?.Invoke();
        }
        
        public T GetRaycastMousePos<T>(Vector3 pos, LayerMask layerMask)
        {
            T res = default;
            Ray ray = mainCamera.ScreenPointToRay(pos);
            int hitCount = Physics.RaycastNonAlloc(ray, _hitResult, 30, layerMask);
            var closest = GetRaycastHitClosestToCamera(hitCount, mainCamera.Camera);
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

        public (RaycastHit, bool) GetRaycastHitClosestToCamera(int hitCount, Camera camera)
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

        public void MoveAgentsTo(GroundBox box)
        {
            foreach (var agent in _agents)
            {
                agent.MoveTo(box.TopCenter);
            }
        }

        public void DestroyGameObject(GameObject target)
        {
            Destroy(target);
        }

        public void SaveDecorations()
        {
            var decors = GetComponentsInChildren<Decoration>();
            _groundArea.SaveDecorations(decors);
            _groundArea.SaveGround();
        }
        
        public void ShowLoadingScreenFor(float seconds, string message)
        {
            StartCoroutine(LoadingIEnumerator(seconds, message));
        }

        private IEnumerator LoadingIEnumerator(float seconds, string message)
        {
            UILoading.Instance.Show(message);
            yield return new WaitForSeconds(seconds);
            UILoading.Instance.Hide();
        }

        private void Update()
        {
            _stateMachine.Update();
        }
    }
}
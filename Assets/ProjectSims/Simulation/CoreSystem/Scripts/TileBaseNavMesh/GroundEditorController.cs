using System;
using System.Collections;
using ProjectSims.Simulation.CoreSystem;
using ProjectSims.Simulation.CoreSystem.Scripts.Interface;
using ProjectSims.Simulation.GroundEditorStates;
using ProjectSims.Simulation.Scripts.StateMachine;
using Simulation.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Simulation.GroundEditor
{
    public class GroundEditorController : MonoBehaviour
    {
        private LayerMask _layerMaskGround;
        public LayerMask LayerMaskGround => _layerMaskGround;
        
        private RaycastHit[] _hitResult;
        private Coroutine _coroutineBakeMesh;
        private IAgent[] _agents;

        private StateMachine<GroundEditorController> _stateMachine;
        
        [SerializeField] private Camera _camera;
        public Camera Camera => _camera;
        
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

        [Header("Camera Speed")] 
        [SerializeField]
        private float _camSpeed = 5;
        public float CamSpeed => _camSpeed;

        private void Awake()
        {
            _stateMachine = new StateMachine<GroundEditorController>(this);
            
            _layerMaskGround = LayerMask.GetMask("Ground");
            _hitResult = new RaycastHit[32];
        }
        
        private void Start()
        {
            UILoading.Instance.Hide();
            _groundArea.LoadGround();
            _popupGroundFileEditor.Hide();
            _uiGroundEditorEdit.Hide();

            _agents = GetComponentsInChildren<IAgent>();
            foreach (var agent in _agents)
            {
                agent.DisableAgent();
            }
            
            _menu.Show();
            BakeNavMesh(() =>
            {
                foreach (var agent in _agents)
                {
                    agent.EnableAgent();
                }
            });
            
            _stateMachine.ChangeState(new GroundEditorNormalState());
        }
        
        public void BakeNavMesh(Action onComplete = null)
        {
            if (_coroutineBakeMesh != null)
            {
                StopCoroutine(_coroutineBakeMesh);
            }

            _coroutineBakeMesh = StartCoroutine(StartBakeNavMesh(onComplete));
        }

        public void ChangeState(IState<GroundEditorController> state)
        {
            _stateMachine.ChangeState(state);
        }

        public void MoveCameraByDragging(Vector3 direction, float speed)
        {
            var camTr = Camera.transform;
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
            Ray ray = Camera.ScreenPointToRay(pos);
            int hitCount = Physics.RaycastNonAlloc(ray, _hitResult, 30, layerMask);
            var closest = GetRaycastHitClosestToCamera(hitCount, Camera);
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

        private void Update()
        {
            _stateMachine.Update();
        }
    }
}
using System;
using System.Collections;
using Simulation.GroundEditor;
using Simulation.UI;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ProjectSims.Simulation.CoreSystem
{
    public class GroundArea : MonoBehaviour
    {
        public enum GroundType
        {
            Pavement,
            Grass,
        }

        public static GroundArea Instance { get; private set; }

        [SerializeField] private GroundData _data;
        [SerializeField] private GroundBox _grassBoxTemplate;
        [SerializeField] private GroundBox _pavementBoxTemplate;
        [SerializeField] private NavMeshSurface _navMeshSurface;

        [SerializeField] private Transform _groundContainer;
        
        [Header("Decoration Settings")]
        [SerializeField] private DecorationSO _decorationSo;
        [field: SerializeField] public Transform TrDecoration { get; private set; }
        private void Awake()
        {
            Instance = this;
            _data.Init();
        }

        public bool IsPointInsideBoundary(Vector3 point)
        {
            var area = _data.Area;
            if (point.x < 0 || point.x > area.x) { return false; }
            if (point.z > 0 || point.z < -area.y) { return false; }

            return true;
        }

        public Vector3 GetRandomPoint()
        {
            float x = Random.Range(-_data.Area.x, _data.Area.x);
            float z = Random.Range(-_data.Area.y, _data.Area.y);
            return new Vector3(x, 1, z);
        }

        private void OnDrawGizmos()
        {
            var topLeft = transform.position + Vector3.right * _data.Area.x * 0.5f + Vector3.back * _data.Area.y * 0.5f;
            Gizmos.DrawWireCube(topLeft, new Vector3(_data.Area.x, 1f, _data.Area.y));
        }

        public void GenerateDefaultGround(int x, int y)
        {
            _data.SetupNewGround(x, y);
        }

        public void SaveGround()
        {
            _data.Save();
        }

        public void SaveDecorations(Decoration[] decor)
        {
            _data._save.Decorations = new GroundData.DecorationDetail[decor.Length];
            for (int i = 0; i < decor.Length; i++)
            {
                GroundData.DecorationDetail d = new GroundData.DecorationDetail();
                d.Name = decor[i].Name;
                d.TransformData.Position = decor[i].transform.position;
                d.TransformData.Rotation = decor[i].transform.rotation.eulerAngles;
                _data._save.Decorations[i] = d;
            }
        }

        public Coroutine LoadGround()
        {
            return StartCoroutine(SpawnGround());
        }

        public Coroutine LoadDecorations()
        {
            return StartCoroutine(SpawnDecorations());
        }

        private IEnumerator SpawnGround()
        {
            UILoading.Instance.Show();
            for (int i = _groundContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(_groundContainer.GetChild(i).gameObject);
            }
            
            var grounds = _data._save.Grounds;
            for (int i = 0; i < grounds.Length; i++)
            {
                GroundBox box = null;
                switch (grounds[i].GroundType)
                {
                    case GroundType.Grass:
                        box = Instantiate(_grassBoxTemplate, _groundContainer);
                        break;

                    case GroundType.Pavement:
                        box = Instantiate(_pavementBoxTemplate, _groundContainer);
                        break;
                }

                box.transform.localPosition = grounds[i].Position;
                box.SetIndex(grounds[i].IndexV2);
                if (i % _data.Area.x == 0) { yield return null;}
            }
            
            UILoading.Instance.Hide();
        }

        private IEnumerator SpawnDecorations()
        {
            UILoading.Instance.Show("Generating Items.");
            var decorData = _data._save.Decorations;
            
            for (int i = 0; i < decorData.Length; i++)
            {
                var d = decorData[i];
                var go = _decorationSo.GetAsset(d.Name);
                if (!go) { continue;}

                Instantiate(go, d.TransformData.Position, Quaternion.Euler(d.TransformData.Rotation), TrDecoration);
                yield return null;
            }
            UILoading.Instance.Hide();
        }
        
        public T SpawnDecoration<T>(GameObject go, Vector3 position, Quaternion quartenion) where T : UnityEngine.Object
        {
            var instGo = Instantiate(go, position, quartenion, TrDecoration);
            
            return instGo.GetComponentInParent<T>();;
        }

        public void SwapToPavementBox(GroundBox go, GroundType groundType)
        {
            GroundBox spawned = null;
            switch (groundType)
            {
                case GroundType.Grass:
                    spawned = Instantiate(_grassBoxTemplate, _groundContainer);
                    break;

                case GroundType.Pavement:
                    spawned = Instantiate(_pavementBoxTemplate, _groundContainer);
                    break;
            }

            spawned.transform.position = go.transform.position;
            spawned.transform.rotation = go.transform.rotation;
            spawned.transform.SetSiblingIndex(go.transform.GetSiblingIndex());
            spawned.SetIndex(go.Index);

            _data.ChangeGroundAtIndex(spawned);

            Destroy(go.gameObject);
        }

        public void BakeNavMesh()
        {
            _navMeshSurface.BuildNavMesh();
        }
    }
}
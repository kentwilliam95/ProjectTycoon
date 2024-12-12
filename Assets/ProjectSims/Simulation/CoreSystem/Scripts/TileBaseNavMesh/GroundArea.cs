using System;
using System.Collections;
using Simulation.GroundEditor;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Serialization;
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

        private void Awake()
        {
            Instance = this;
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

        private IEnumerator DelayPerFrame(int frameCount, Action onComplete)
        {
            for (int i = 0; i < frameCount; i++)
            {
                yield return null;
            }

            onComplete?.Invoke();
        }

        public void GenerateDefaultGround()
        {
            _data.ClearSaveData();
            
            Vector3 startPos = new Vector3(0, 0, 0);
            for (int i = 0; i < _data.Area.y; i++)
            {
                for (int j = 0; j < _data.Area.x; j++)
                {
                    _data.SetupGroundBox(new Vector2Int(j, i), startPos, GroundType.Grass);
                    startPos.x += 1f;
                }
            
                startPos.x = 0;
                startPos.z -= 1f;
            }
        }

        public void LoadGround()
        {
            StartCoroutine(SpawnGround());
        }

        private IEnumerator SpawnGround()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            yield return null;
            var grounds = _data._save.Grounds;
            for (int i = 0; i < grounds.Length; i++)
            {
                GroundBox box = null;
                switch (grounds[i].GroundType)
                {
                    case GroundType.Grass:
                        box = Instantiate(_grassBoxTemplate, transform);
                        break;
                    
                    case GroundType.Pavement:
                        box = Instantiate(_pavementBoxTemplate, transform);
                        break;
                }

                box.transform.localPosition = grounds[i].LocalPosition;
                box.SetIndex(grounds[i].IndexV2);
            }
        }

        public void SwapToPavementBox(GroundBox go, GroundType groundType)
        {
            GroundBox spawned = null;
            switch (groundType)
            {
                case GroundType.Grass:
                    spawned = Instantiate(_grassBoxTemplate, transform);
                    break;

                case GroundType.Pavement:
                    spawned = Instantiate(_pavementBoxTemplate, transform);
                    break;
            }

            spawned.transform.position = go.transform.position;
            spawned.transform.rotation = go.transform.rotation;
            spawned.transform.SetSiblingIndex(go.transform.GetSiblingIndex());
            spawned.SetIndex(go.Index);
            
            _data.ChangeGroundAtIndex(spawned);

            Destroy(go.gameObject);
        }

    }
}
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectSims.Simulation.CoreSystem
{
    public class GroundArea : MonoBehaviour
    {
        public static GroundArea Instance { get; private set; }
        [SerializeField] private Vector3 _area;

        private void Awake()
        {
            Instance = this;
        }

        public Vector3 GetRandomPoint()
        {
            float x = Random.Range(-_area.x, _area.x);
            float y = Random.Range(-_area.y, _area.y);
            float z = Random.Range(-_area.z, _area.z);
            return new Vector3(x, y, z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(Vector3.zero, _area);
        }
    }
}
using UnityEngine;

namespace Simulation.GroundEditor
{
    [CreateAssetMenu(menuName = "Simulation/Data/Decoration")]
    public class DecorationSO : ScriptableObject
    {
        [System.Serializable]
        public struct AssetDetail
        {
            public Sprite Sprite;
            public GameObject Template;   
        }

        public AssetDetail[] Asset;
    }   
}

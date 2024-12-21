using System;
using System.Collections.Generic;
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

        private Dictionary<String, GameObject> _dictAsset;

        public void InitDictionary()
        {
            _dictAsset = new Dictionary<string, GameObject>();
            for (int i = 0; i < Asset.Length; i++)
            {
                var asset = Asset[i];
                _dictAsset.TryAdd(asset.Template.name, asset.Template);
            }
        }

        public GameObject GetAsset(string assetName)
        {
            if (!_dictAsset.ContainsKey(assetName))
            {
                return null;
            }

            return _dictAsset[assetName];
        }
    }   
}

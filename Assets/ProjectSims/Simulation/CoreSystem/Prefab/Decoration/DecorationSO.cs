using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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

#if UNITY_EDITOR
        [Button("Setup")]
        public void Setup()
        {
            string[] files = Directory.GetFiles("Assets/ProjectSims/Simulation/CoreSystem/Prefab/Decoration/", "*.prefab", SearchOption.TopDirectoryOnly);
            
            Asset = new AssetDetail[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                var decor = AssetDatabase.LoadAssetAtPath<Decoration>(files[i]);
                Asset[i].Template = decor.gameObject;

            }
        }
#endif
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProjectSims.Simulation.CoreSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Simulation.GroundEditor
{
    [CreateAssetMenu(menuName = "Simulation/Data/GroundData")]
    public class GroundData : ScriptableObject
    {
        private const string SAVEKEY = "GroundData";

        [System.Serializable]
        public struct SaveData
        {
            public Vector2Int Area;
            public GroundDetail[] Grounds;
            public DecorationDetail[] Decorations;
        }

        [System.Serializable]
        public struct GroundDetail
        {
            public GroundArea.GroundType GroundType;
            public Vector2Int IndexV2;
            public int Index;
            
            public TransformData TransformData;
            public Vector3 Position => TransformData.Position;
        }
        
        [System.Serializable]
        public struct DecorationDetail
        {
            public string Name;
            public TransformData TransformData;
        }
        
        [System.Serializable]
        public struct TransformData
        {
            public Vector3 Position;
            public Vector3 Rotation;
        }

        public SaveData _save;
        public Vector2Int Area => _save.Area;

        private Dictionary<String, GameObject> _dictAsset;

        public void InitDictionary()
        {
            _dictAsset = new Dictionary<string, GameObject>();
        }
        
        public void Init()
        {
            var str = PlayerPrefs.GetString(SAVEKEY);
            if (!String.IsNullOrEmpty(str))
            {
                _save = JsonUtility.FromJson<SaveData>(str);
            }
            else
            {
                SetupNewGround(10, 10);
            }
            
            
        }

        public void Save()
        {
            PlayerPrefs.SetString(SAVEKEY, JsonUtility.ToJson(_save));
        }

        public void ClearSaveData()
        {
            _save.Grounds = new GroundDetail[Area.x * Area.y];
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
#endif
        }

        public void ChangeGroundAtIndex(GroundBox change)
        {
            var indexV2 = change.Index;
            var indexV1 = indexV2.y * Area.x + indexV2.x;

            if (indexV1 >= _save.Grounds.Length)
            {
                Debug.Log($"[GroundData] could not change data, invalid index.");
            }

            var groundDetail = _save.Grounds[indexV1];
            groundDetail.GroundType = change.GroundType;
            groundDetail.IndexV2 = change.Index;
            groundDetail.TransformData.Position = change.transform.localPosition;
            groundDetail.TransformData.Rotation = change.transform.rotation.eulerAngles;

            _save.Grounds[indexV1] = groundDetail;

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
#endif
        }

        public void SetupNewGround(int x, int y)
        {
            _save.Area = new Vector2Int(x, y);
            ClearSaveData();
            Vector3 startPos = new Vector3(0, 0, 0);
            for (int i = 0; i < Area.y; i++)
            {
                for (int j = 0; j < Area.x; j++)
                {
                    SetupGroundBox(new Vector2Int(j, i), startPos, GroundArea.GroundType.Grass);
                    startPos.x += 1f;
                }

                startPos.x = 0;
                startPos.z -= 1f;
            }

            Save();
        }

        private void SetupGroundBox(Vector2Int index, Vector3 localPosition, GroundArea.GroundType groundType)
        {
            var indexV1 = index.y * Area.x + index.x;
            _save.Grounds[indexV1].GroundType = groundType;
            _save.Grounds[indexV1].TransformData.Position = localPosition;
            _save.Grounds[indexV1].IndexV2 = index;
            _save.Grounds[indexV1].Index = indexV1;
        }
    }
}
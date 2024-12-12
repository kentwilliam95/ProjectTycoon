using System;
using System.Collections;
using System.Collections.Generic;
using ProjectSims.Simulation.CoreSystem;
using UnityEngine;

namespace Simulation.GroundEditor
{
    [CreateAssetMenu(menuName = "Simulation/Data/GroundData")]
    public class GroundData : ScriptableObject
    {
        [System.Serializable]
        public struct SaveData
        {
            public Vector2Int Area;
            public GroundDetail[] Grounds;
        }
        
        [System.Serializable]
        public struct GroundDetail
        {
            public GroundArea.GroundType GroundType;
            public Vector2Int IndexV2;
            public int Index;
            
            public Vector3 LocalPosition;
            public Vector3 Rotation;
        }
        
        public SaveData _save;
        public Vector2Int Area => _save.Area;

        public void ClearSaveData()
        {
            // _save.Grounds.Clear();
            _save.Grounds = new GroundDetail[Area.x * Area.y];
        }

        public void ChangeGroundAtIndex(GroundBox change)
        {
            var indexV2 = change.Index;
            var indexV1 = indexV2.y * Area.x + indexV2.x;

            if (indexV1 >= _save.Grounds.Length) { Debug.Log($"[GroundData] could not change data, invalid index."); }

            var groundDetail = _save.Grounds[indexV1];
            groundDetail.GroundType = change.GroundType;
            groundDetail.IndexV2 = change.Index;
            groundDetail.LocalPosition = change.transform.localPosition;
            groundDetail.Rotation = change.transform.rotation.eulerAngles;

            _save.Grounds[indexV1] = groundDetail;
        }

        public void SetupGroundBox(Vector2Int index, Vector3 localPosition, GroundArea.GroundType groundType)
        {
            var indexV1 = index.y * Area.x + index.x;
            _save.Grounds[indexV1].GroundType = groundType;
            _save.Grounds[indexV1].LocalPosition = localPosition;
            _save.Grounds[indexV1].IndexV2 = index;
            _save.Grounds[indexV1].Index = indexV1;
        }
    }   
}
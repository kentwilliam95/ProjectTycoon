using UnityEditor;
using UnityEngine;

namespace Simulation.GroundEditor.Editor
{
    public class EditorCreatePrefab : MonoBehaviour
    {
        [MenuItem("Window/Create Prefab")]
        public static void CreatePrefabFromSprite()
        {
            var gos = Selection.objects;
            int i = 0;
            foreach (var item in gos)
            {
                var go = new GameObject();
                go.transform.position = Vector3.zero;
                go.name = "obj" + i++;
                var child = new GameObject();
                child.transform.SetParent(go.transform);
                child.transform.localScale = Vector3.one * 0.3f;
                
                var sr = child.AddComponent<SpriteRenderer>();
                sr.sprite = (Sprite)item;
                sr.spriteSortPoint = SpriteSortPoint.Pivot;

                var path = "Assets/Prefabs/" + go.name + ".prefab";
                PrefabUtility.SaveAsPrefabAssetAndConnect(go, path, InteractionMode.AutomatedAction);
            }
        }
    }
}
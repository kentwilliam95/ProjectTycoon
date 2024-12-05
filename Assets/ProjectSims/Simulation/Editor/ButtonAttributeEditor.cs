using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomEditor(typeof(UnityEngine.Object), true)]
public class ButtonAttributeEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get the current object
        var targetObject = target;

        // Get all methods from the target object
        var methods = targetObject.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var method in methods)
        {
            // Check if the method has the ButtonAttribute
            var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
            if (buttonAttribute != null)
            {
                string buttonText = buttonAttribute.ButtonText ?? method.Name;

                if (GUILayout.Button(buttonText))
                {
                    // Invoke the method when the button is clicked
                    method.Invoke(targetObject, null);
                }
            }
        }
    }
}
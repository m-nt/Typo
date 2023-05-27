using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(DictionaryWords))]
public class DictionaryWordsEditor : Editor
{
    private SerializedProperty serializedProperty;


    private void OnEnable()
    {


        // Get the serialized property for the dictionary field
        // serializedObject = new SerializedObject(targetObject);

        serializedProperty = serializedObject.FindProperty("words.Array");


    }


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        // Update the serialized object to show changes made in the inspector
        serializedObject.Update();

        if (GUILayout.Button("Load Dictionary"))
        {
            DictionaryWords myScriptableObject = (DictionaryWords)target;
            myScriptableObject.LoadFiles();
            AssetDatabase.SaveAssets();
            serializedObject.ApplyModifiedProperties();
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        }
        // Apply any changes made in the inspector to the serialized object
        serializedObject.ApplyModifiedProperties();
    }

}
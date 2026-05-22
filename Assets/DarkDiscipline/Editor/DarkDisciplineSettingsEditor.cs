using DarkDiscipline.Runtime;
using UnityEditor;

namespace DarkDiscipline.Editor
{
    [CustomEditor(typeof(DarkDisciplineSettings))]
    public sealed class DarkDisciplineSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultStartingWeightKg"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultTargetRepetitions"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("weightProgressionStepKg"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dailyCalorieTarget"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dailyProteinTargetGrams"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dailyCarbohydrateTargetGrams"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dailyFatTargetGrams"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}

using System.IO;
using DarkDiscipline.Runtime;
using UnityEditor;
using UnityEngine;

namespace DarkDiscipline.Editor
{
    public sealed class DarkDisciplineSetupWizard : EditorWindow
    {
        private const string SettingsDirectory = "Assets/DarkDiscipline/Resources";
        private const string SettingsPath = SettingsDirectory + "/DarkDisciplineSettings.asset";
        private const string MenuPath = "Tools/Dark Discipline/Setup Wizard";

        [MenuItem(MenuPath)]
        public static void Open()
        {
            GetWindow<DarkDisciplineSetupWizard>("Dark Discipline");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Project Setup", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Creates the runtime settings asset used by VContainer registrations.", MessageType.Info);

            if (GUILayout.Button("Create Runtime Settings"))
            {
                CreateSettingsAsset();
            }
        }

        private static void CreateSettingsAsset()
        {
            if (AssetDatabase.LoadAssetAtPath<DarkDisciplineSettings>(SettingsPath) != null)
            {
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<DarkDisciplineSettings>(SettingsPath);
                return;
            }

            Directory.CreateDirectory(SettingsDirectory);
            var settings = CreateInstance<DarkDisciplineSettings>();
            AssetDatabase.CreateAsset(settings, SettingsPath);
            AssetDatabase.SaveAssets();
            Selection.activeObject = settings;
        }
    }
}

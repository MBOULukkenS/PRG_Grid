using UnityEditor;
using UnityEngine;

namespace Editors
{
    [CustomEditor(typeof(GridRoadGenerator))]
    public class GridRoadGeneratorEditor : Editor
    {
        private GridRoadGenerator _generator;
        
        private void OnEnable()
        {
            _generator = target as GridRoadGenerator;
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            using (EditorGUI.DisabledGroupScope disabledGroupScope = new EditorGUI.DisabledGroupScope(!Application.isPlaying))
            {
                if (GUILayout.Button("Update Road"))
                    _generator.GenerateRoad();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
using Grid;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [CustomEditor(typeof(TileGrid))]
    public class GridEditor : Editor
    {
        private TileGrid _grid;
        private SerializedProperty _widthProperty;
        private SerializedProperty _heightProperty;
        
        private void OnEnable()
        {
            _grid = target as TileGrid;
            
            _widthProperty = serializedObject.FindProperty("_width");
            _heightProperty = serializedObject.FindProperty("_height");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            using (EditorGUI.ChangeCheckScope scope = new EditorGUI.ChangeCheckScope())
            {
                Vector2Int newGridSize = EditorGUILayout.Vector2IntField("Grid Size", _grid.GridSize);
                
                if (scope.changed)
                {
                    Undo.RecordObject(target, nameof(_grid.GridSize));
                    
                    _widthProperty.intValue = newGridSize.x;
                    _heightProperty.intValue = newGridSize.y;
                    
                    //_grid.GridSize = newGridSize;
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace com.biscrat.AnimatorControllerMerger.Editor
{
    [CustomEditor(typeof(AnimatorControllerMergeSetting))]
    [CanEditMultipleObjects]
    public class AnimatorControllerMergeSettingEditor : UnityEditor.Editor
    {
        private SerializedProperty _srcProperty;
        private SerializedProperty _destProperty;

        private void OnEnable()
        {
            _srcProperty = serializedObject.FindProperty(AnimatorControllerMergeSetting.FieldNames.Src);
            _destProperty = serializedObject.FindProperty(AnimatorControllerMergeSetting.FieldNames.Dest);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var hasError = false;

            {
                EditorGUILayout.PropertyField(_srcProperty, new GUIContent("Source Animator Controllers"));
                if (_srcProperty.arraySize == 0)
                {
                    hasError = true;
                    EditorGUILayout.HelpBox("No source animator controllers selected.", MessageType.Error);
                }
                else
                {
                    var hasNull = false;
                    for (var i = 0; i < _srcProperty.arraySize; ++i)
                    {
                        var animatorController = _srcProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                        if (animatorController is null) hasNull = true;
                    }

                    if (hasNull)
                    {
                        hasError = true;
                        EditorGUILayout.HelpBox("Some source animator controllers are null.", MessageType.Error);
                    }
                }
            }
            
            EditorGUILayout.Space();

            {
                EditorGUILayout.PropertyField(_destProperty, new GUIContent("Destination Animator Controller"));
                if (_destProperty.objectReferenceValue is null)
                {
                    hasError = true;
                    EditorGUILayout.HelpBox("No destination animator controller selected.", MessageType.Error);
                }
            }

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            {
                EditorGUI.BeginDisabledGroup(hasError);
                if (GUILayout.Button("Merge"))
                {
                    var setting = (AnimatorControllerMergeSetting)target;
                    setting.Merge();
                }

                EditorGUI.EndDisabledGroup();
            }
        }
    }
}
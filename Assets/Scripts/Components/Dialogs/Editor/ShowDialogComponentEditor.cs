using PixelCrew.Utils.Disposables.Editor;
using UnityEditor;

namespace Components.Dialogs.Editor
{
    [CustomEditor(typeof(ShowDialogComponent))]
    public class ShowDialogComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;
        private SerializedProperty _onCompleteProperty;

        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");
            _onCompleteProperty = serializedObject.FindProperty("_onComplete");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);
            if (_modeProperty.GetEnum(out ShowDialogComponent.Mode mode))
            {
                switch (mode)
                {
                    case ShowDialogComponent.Mode.Bound:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_bound"));
                        break;
                    case ShowDialogComponent.Mode.External:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_external"));
                        break;
                }
            }
            EditorGUILayout.PropertyField(_onCompleteProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace UnityEditor.PostProcessing
{
    public class PostProcessingModelEditor
    {
        public SerializedProperty serializedProperty { get; internal set; }
        public PostProcessingModel target { get; internal set; }
        internal bool alwaysEnabled = false;
        internal PostProcessingInspector inspector;
        internal PostProcessingProfile profile;

        protected SerializedProperty m_EnabledProperty;
        protected SerializedProperty m_SettingsProperty;

        public virtual void OnDisable()
        { }

        public virtual void OnEnable()
        { }

        public virtual void OnInspectorGUI()
        { }

        public void Repaint()
        {
            inspector.Repaint();
        }

        internal void OnGUI()
        {
            GUILayout.Space(5);

            var display = alwaysEnabled
                ? EditorGUIHelper.Header(serializedProperty.displayName, m_SettingsProperty, Reset)
                : EditorGUIHelper.Header(serializedProperty.displayName, m_SettingsProperty, m_EnabledProperty, Reset);

            if (display)
            {
                EditorGUI.indentLevel++;
                using (new EditorGUI.DisabledGroupScope(!m_EnabledProperty.boolValue))
                {
                    OnInspectorGUI();
                }
                EditorGUI.indentLevel--;
            }
        }

        internal void OnPreEnable()
        {
            m_SettingsProperty = serializedProperty.FindPropertyRelative("m_Settings");
            m_EnabledProperty = serializedProperty.FindPropertyRelative("m_Enabled");

            OnEnable();
        }

        protected SerializedProperty FindSetting<T, TValue>(Expression<Func<T, TValue>> expr)
        {
            return m_SettingsProperty.FindPropertyRelative(ReflectionUtils.GetFieldPath(expr));
        }

        protected SerializedProperty FindSetting<T, TValue>(SerializedProperty prop, Expression<Func<T, TValue>> expr)
        {
            return prop.FindPropertyRelative(ReflectionUtils.GetFieldPath(expr));
        }

        private void Reset()
        {
            var obj = serializedProperty.serializedObject;
            Undo.RecordObject(obj.targetObject, "Reset");
            target.Reset();
            EditorUtility.SetDirty(obj.targetObject);
        }
    }
}
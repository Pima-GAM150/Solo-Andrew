using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace UnityEditor.PostProcessing
{
    public class PostProcessingModelEditor
    {
        #region Internal Fields

        internal bool alwaysEnabled = false;
        internal PostProcessingInspector inspector;
        internal PostProcessingProfile profile;

        #endregion Internal Fields

        #region Protected Fields

        protected SerializedProperty m_EnabledProperty;
        protected SerializedProperty m_SettingsProperty;

        #endregion Protected Fields

        #region Public Properties

        public SerializedProperty serializedProperty { get; internal set; }
        public PostProcessingModel target { get; internal set; }

        #endregion Public Properties

        #region Public Methods

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

        #endregion Public Methods

        #region Internal Methods

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

        #endregion Internal Methods

        #region Protected Methods

        protected SerializedProperty FindSetting<T, TValue>(Expression<Func<T, TValue>> expr)
        {
            return m_SettingsProperty.FindPropertyRelative(ReflectionUtils.GetFieldPath(expr));
        }

        protected SerializedProperty FindSetting<T, TValue>(SerializedProperty prop, Expression<Func<T, TValue>> expr)
        {
            return prop.FindPropertyRelative(ReflectionUtils.GetFieldPath(expr));
        }

        #endregion Protected Methods

        #region Private Methods

        private void Reset()
        {
            var obj = serializedProperty.serializedObject;
            Undo.RecordObject(obj.targetObject, "Reset");
            target.Reset();
            EditorUtility.SetDirty(obj.targetObject);
        }

        #endregion Private Methods
    }
}
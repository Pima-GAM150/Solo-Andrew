using System.Collections.Generic;

namespace UnityEditor.PostProcessing
{
    public class DefaultPostFxModelEditor : PostProcessingModelEditor
    {
        #region Private Fields

        private List<SerializedProperty> m_Properties = new List<SerializedProperty>();

        #endregion Private Fields

        #region Public Methods

        public override void OnEnable()
        {
            var iter = m_SettingsProperty.Copy().GetEnumerator();
            while (iter.MoveNext())
                m_Properties.Add(((SerializedProperty)iter.Current).Copy());
        }

        public override void OnInspectorGUI()
        {
            foreach (var property in m_Properties)
                EditorGUILayout.PropertyField(property);
        }

        #endregion Public Methods
    }
}
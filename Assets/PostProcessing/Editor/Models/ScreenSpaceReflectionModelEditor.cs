using UnityEngine.PostProcessing;

namespace UnityEditor.PostProcessing
{
    using Settings = ScreenSpaceReflectionModel.Settings;

    [PostProcessingModelEditor(typeof(ScreenSpaceReflectionModel))]
    public class ScreenSpaceReflectionModelEditor : PostProcessingModelEditor
    {
        #region Private Fields

        private IntensitySettings m_Intensity;

        private ReflectionSettings m_Reflection;

        private ScreenEdgeMask m_ScreenEdgeMask;

        #endregion Private Fields

        #region Public Methods

        public override void OnEnable()
        {
            m_Intensity = new IntensitySettings
            {
                reflectionMultiplier = FindSetting((Settings x) => x.intensity.reflectionMultiplier),
                fadeDistance = FindSetting((Settings x) => x.intensity.fadeDistance),
                fresnelFade = FindSetting((Settings x) => x.intensity.fresnelFade),
                fresnelFadePower = FindSetting((Settings x) => x.intensity.fresnelFadePower)
            };

            m_Reflection = new ReflectionSettings
            {
                blendType = FindSetting((Settings x) => x.reflection.blendType),
                reflectionQuality = FindSetting((Settings x) => x.reflection.reflectionQuality),
                maxDistance = FindSetting((Settings x) => x.reflection.maxDistance),
                iterationCount = FindSetting((Settings x) => x.reflection.iterationCount),
                stepSize = FindSetting((Settings x) => x.reflection.stepSize),
                widthModifier = FindSetting((Settings x) => x.reflection.widthModifier),
                reflectionBlur = FindSetting((Settings x) => x.reflection.reflectionBlur),
                reflectBackfaces = FindSetting((Settings x) => x.reflection.reflectBackfaces)
            };

            m_ScreenEdgeMask = new ScreenEdgeMask
            {
                intensity = FindSetting((Settings x) => x.screenEdgeMask.intensity)
            };
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This effect only works with the deferred rendering path.", MessageType.Info);

            EditorGUILayout.LabelField("Reflection", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_Reflection.blendType);
            EditorGUILayout.PropertyField(m_Reflection.reflectionQuality);
            EditorGUILayout.PropertyField(m_Reflection.maxDistance);
            EditorGUILayout.PropertyField(m_Reflection.iterationCount);
            EditorGUILayout.PropertyField(m_Reflection.stepSize);
            EditorGUILayout.PropertyField(m_Reflection.widthModifier);
            EditorGUILayout.PropertyField(m_Reflection.reflectionBlur);
            EditorGUILayout.PropertyField(m_Reflection.reflectBackfaces);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Intensity", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_Intensity.reflectionMultiplier);
            EditorGUILayout.PropertyField(m_Intensity.fadeDistance);
            EditorGUILayout.PropertyField(m_Intensity.fresnelFade);
            EditorGUILayout.PropertyField(m_Intensity.fresnelFadePower);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Screen Edge Mask", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_ScreenEdgeMask.intensity);
            EditorGUI.indentLevel--;
        }

        #endregion Public Methods

        #region Private Structs

        private struct IntensitySettings
        {
            #region Public Fields

            public SerializedProperty fadeDistance;
            public SerializedProperty fresnelFade;
            public SerializedProperty fresnelFadePower;
            public SerializedProperty reflectionMultiplier;

            #endregion Public Fields
        }

        private struct ReflectionSettings
        {
            #region Public Fields

            public SerializedProperty blendType;
            public SerializedProperty iterationCount;
            public SerializedProperty maxDistance;
            public SerializedProperty reflectBackfaces;
            public SerializedProperty reflectionBlur;
            public SerializedProperty reflectionQuality;
            public SerializedProperty stepSize;
            public SerializedProperty widthModifier;

            #endregion Public Fields
        }

        private struct ScreenEdgeMask
        {
            #region Public Fields

            public SerializedProperty intensity;

            #endregion Public Fields
        }

        #endregion Private Structs
    }
}
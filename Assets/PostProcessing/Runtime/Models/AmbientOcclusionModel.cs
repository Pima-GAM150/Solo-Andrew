using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class AmbientOcclusionModel : PostProcessingModel
    {
        [Serializable]
        public struct Settings
        {
            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        intensity = 1f,
                        radius = 0.3f,
                        sampleCount = SampleCount.Medium,
                        downsampling = true,
                        forceForwardCompatibility = false,
                        ambientOnly = false,
                        highPrecision = false
                    };
                }
            }

            [Tooltip("Enables the ambient-only mode in that the effect only affects ambient lighting. This mode is only available with the Deferred rendering path and HDR rendering.")]
            public bool ambientOnly;

            [Tooltip("Halves the resolution of the effect to increase performance at the cost of visual quality.")]
            public bool downsampling;

            [Tooltip("Forces compatibility with Forward rendered objects when working with the Deferred rendering path.")]
            public bool forceForwardCompatibility;

            [Tooltip("Toggles the use of a higher precision depth texture with the forward rendering path (may impact performances). Has no effect with the deferred rendering path.")]
            public bool highPrecision;

            [Range(0, 4), Tooltip("Degree of darkness produced by the effect.")]
            public float intensity;

            [Min(1e-4f), Tooltip("Radius of sample points, which affects extent of darkened areas.")]
            public float radius;

            [Tooltip("Number of sample points, which affects quality and performance.")]
            public SampleCount sampleCount;
        }

        public enum SampleCount
        {
            Lowest = 3,
            Low = 6

,
            Medium = 10,
            High = 16
        }

        public Settings settings
        {
            get { return m_Settings; }
            set { m_Settings = value; }
        }

        [SerializeField]
        private Settings m_Settings = Settings.defaultSettings;

        public override void Reset()
        {
            m_Settings = Settings.defaultSettings;
        }
    }
}
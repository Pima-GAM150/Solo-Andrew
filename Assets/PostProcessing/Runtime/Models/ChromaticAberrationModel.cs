using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class ChromaticAberrationModel : PostProcessingModel
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
                        spectralTexture = null,
                        intensity = 0.1f
                    };
                }
            }

            [Range(0f, 1f), Tooltip("Amount of tangential distortion.")]
            public float intensity;

            [Tooltip("Shift the hue of chromatic aberrations.")]
            public Texture2D spectralTexture;
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
using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class UserLutModel : PostProcessingModel
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
                        lut = null,
                        contribution = 1f
                    };
                }
            }

            [Range(0f, 1f), Tooltip("Blending factor.")]
            public float contribution;

            [Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
            public Texture2D lut;
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
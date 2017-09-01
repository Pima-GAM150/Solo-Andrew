using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class ChromaticAberrationModel : PostProcessingModel
    {
        #region Private Fields

        [SerializeField]
        private Settings m_Settings = Settings.defaultSettings;

        #endregion Private Fields

        #region Public Properties

        public Settings settings
        {
            get { return m_Settings; }
            set { m_Settings = value; }
        }

        #endregion Public Properties

        #region Public Methods

        public override void Reset()
        {
            m_Settings = Settings.defaultSettings;
        }

        #endregion Public Methods

        #region Public Structs

        [Serializable]
        public struct Settings
        {
            #region Public Fields

            [Range(0f, 1f), Tooltip("Amount of tangential distortion.")]
            public float intensity;

            [Tooltip("Shift the hue of chromatic aberrations.")]
            public Texture2D spectralTexture;

            #endregion Public Fields

            #region Public Properties

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

            #endregion Public Properties
        }

        #endregion Public Structs
    }
}
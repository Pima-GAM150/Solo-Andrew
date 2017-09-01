using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class GrainModel : PostProcessingModel
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

            [Tooltip("Enable the use of colored grain.")]
            public bool colored;

            [Range(0f, 1f), Tooltip("Grain strength. Higher means more visible grain.")]
            public float intensity;

            [Range(0f, 1f), Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
            public float luminanceContribution;

            [Range(0.3f, 3f), Tooltip("Grain particle size.")]
            public float size;

            #endregion Public Fields

            #region Public Properties

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        colored = true,
                        intensity = 0.5f,
                        size = 1f,
                        luminanceContribution = 0.8f
                    };
                }
            }

            #endregion Public Properties
        }

        #endregion Public Structs
    }
}
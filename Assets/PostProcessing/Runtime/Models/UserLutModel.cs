using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class UserLutModel : PostProcessingModel
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

            [Range(0f, 1f), Tooltip("Blending factor.")]
            public float contribution;

            [Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
            public Texture2D lut;

            #endregion Public Fields

            #region Public Properties

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

            #endregion Public Properties
        }

        #endregion Public Structs
    }
}
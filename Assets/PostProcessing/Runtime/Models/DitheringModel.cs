using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class DitheringModel : PostProcessingModel
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
            #region Public Properties

            public static Settings defaultSettings
            {
                get { return new Settings(); }
            }

            #endregion Public Properties
        }

        #endregion Public Structs
    }
}
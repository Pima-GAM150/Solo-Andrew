using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class FogModel : PostProcessingModel
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
                        excludeSkybox = true
                    };
                }
            }

            [Tooltip("Should the fog affect the skybox?")]
            public bool excludeSkybox;
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
using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class MotionBlurModel : PostProcessingModel
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

            [Range(0f, 1f), Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
            public float frameBlending;

            [Range(4, 32), Tooltip("The amount of sample points, which affects quality and performances.")]
            public int sampleCount;

            [Range(0f, 360f), Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
            public float shutterAngle;

            #endregion Public Fields

            #region Public Properties

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        shutterAngle = 270f,
                        sampleCount = 10,
                        frameBlending = 0f
                    };
                }
            }

            #endregion Public Properties
        }

        #endregion Public Structs
    }
}
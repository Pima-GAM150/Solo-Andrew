using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class EyeAdaptationModel : PostProcessingModel
    {
        #region Private Fields

        [SerializeField]
        private Settings m_Settings = Settings.defaultSettings;

        #endregion Private Fields

        #region Public Enums

        public enum EyeAdaptationType
        {
            #region Public Fields

            Progressive,
            Fixed

            #endregion Public Fields
        }

        #endregion Public Enums

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

            [Tooltip("Use \"Progressive\" if you want the auto exposure to be animated. Use \"Fixed\" otherwise.")]
            public EyeAdaptationType adaptationType;

            [Tooltip("Set this to true to let Unity handle the key value automatically based on average luminance.")]
            public bool dynamicKeyValue;

            [Range(1f, 99f), Tooltip("Filters the bright part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
            public float highPercent;

            [Min(0f), Tooltip("Exposure bias. Use this to offset the global exposure of the scene.")]
            public float keyValue;

            [Range(1, 16), Tooltip("Upper bound for the brightness range of the generated histogram (in EV). The bigger the spread between min & max, the lower the precision will be.")]
            public int logMax;

            [Range(-16, -1), Tooltip("Lower bound for the brightness range of the generated histogram (in EV). The bigger the spread between min & max, the lower the precision will be.")]
            public int logMin;

            [Range(1f, 99f), Tooltip("Filters the dark part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
            public float lowPercent;

            [Tooltip("Maximum average luminance to consider for auto exposure (in EV).")]
            public float maxLuminance;

            [Tooltip("Minimum average luminance to consider for auto exposure (in EV).")]
            public float minLuminance;

            [Min(0f), Tooltip("Adaptation speed from a light to a dark environment.")]
            public float speedDown;

            [Min(0f), Tooltip("Adaptation speed from a dark to a light environment.")]
            public float speedUp;

            #endregion Public Fields

            #region Public Properties

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        lowPercent = 45f,
                        highPercent = 95f,

                        minLuminance = -5f,
                        maxLuminance = 1f,
                        keyValue = 0.25f,
                        dynamicKeyValue = true,

                        adaptationType = EyeAdaptationType.Progressive,
                        speedUp = 2f,
                        speedDown = 1f,

                        logMin = -8,
                        logMax = 4
                    };
                }
            }

            #endregion Public Properties
        }

        #endregion Public Structs
    }
}
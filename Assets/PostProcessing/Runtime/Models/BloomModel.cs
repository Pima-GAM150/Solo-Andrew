using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class BloomModel : PostProcessingModel
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
        public struct BloomSettings
        {
            #region Public Fields

            [Tooltip("Reduces flashing noise with an additional filter.")]
            public bool antiFlicker;

            [Min(0f), Tooltip("Strength of the bloom filter.")]
            public float intensity;

            [Range(1f, 7f), Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
            public float radius;

            [Range(0f, 1f), Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
            public float softKnee;

            [Min(0f), Tooltip("Filters out pixels under this level of brightness.")]
            public float threshold;

            #endregion Public Fields

            #region Public Properties

            public static BloomSettings defaultSettings
            {
                get
                {
                    return new BloomSettings
                    {
                        intensity = 0.5f,
                        threshold = 1.1f,
                        softKnee = 0.5f,
                        radius = 4f,
                        antiFlicker = false,
                    };
                }
            }

            public float thresholdLinear
            {
                set { threshold = Mathf.LinearToGammaSpace(value); }
                get { return Mathf.GammaToLinearSpace(threshold); }
            }

            #endregion Public Properties
        }

        [Serializable]
        public struct LensDirtSettings
        {
            #region Public Fields

            [Min(0f), Tooltip("Amount of lens dirtiness.")]
            public float intensity;

            [Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
            public Texture texture;

            #endregion Public Fields

            #region Public Properties

            public static LensDirtSettings defaultSettings
            {
                get
                {
                    return new LensDirtSettings
                    {
                        texture = null,
                        intensity = 3f
                    };
                }
            }

            #endregion Public Properties
        }

        [Serializable]
        public struct Settings
        {
            #region Public Fields

            public BloomSettings bloom;
            public LensDirtSettings lensDirt;

            #endregion Public Fields

            #region Public Properties

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        bloom = BloomSettings.defaultSettings,
                        lensDirt = LensDirtSettings.defaultSettings
                    };
                }
            }

            #endregion Public Properties
        }

        #endregion Public Structs
    }
}
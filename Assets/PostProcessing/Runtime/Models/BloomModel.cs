using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class BloomModel : PostProcessingModel
    {
        [Serializable]
        public struct BloomSettings
        {
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
        }

        [Serializable]
        public struct LensDirtSettings
        {
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

            [Min(0f), Tooltip("Amount of lens dirtiness.")]
            public float intensity;

            [Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
            public Texture texture;
        }

        [Serializable]
        public struct Settings
        {
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

            public BloomSettings bloom;
            public LensDirtSettings lensDirt;
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
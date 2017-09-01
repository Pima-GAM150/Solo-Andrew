using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class ColorGradingModel : PostProcessingModel
    {
        #region Private Fields

        [SerializeField]
        private Settings m_Settings = Settings.defaultSettings;

        #endregion Private Fields

        #region Public Enums

        public enum ColorWheelMode
        {
            Linear,
            Log
        }

        public enum Tonemapper
        {
            None,

            /// <summary>
            /// ACES Filmic reference tonemapper.
            /// </summary>
            ACES,

            /// <summary>
            /// Neutral tonemapper (based off John Hable's & Jim Hejl's work).
            /// </summary>
            Neutral
        }

        #endregion Public Enums

        #region Public Properties

        public RenderTexture bakedLut { get; internal set; }

        public bool isDirty { get; internal set; }

        public Settings settings
        {
            get
            {
                return m_Settings;
            }
            set
            {
                m_Settings = value;
                OnValidate();
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override void OnValidate()
        {
            isDirty = true;
        }

        public override void Reset()
        {
            m_Settings = Settings.defaultSettings;
            OnValidate();
        }

        #endregion Public Methods

        #region Public Structs

        [Serializable]
        public struct BasicSettings
        {
            #region Public Fields

            [Range(0f, 2f), Tooltip("Expands or shrinks the overall range of tonal values.")]
            public float contrast;

            [Range(-180f, 180f), Tooltip("Shift the hue of all colors.")]
            public float hueShift;

            [Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
            public float postExposure;

            [Range(0f, 2f), Tooltip("Pushes the intensity of all colors.")]
            public float saturation;

            [Range(-100f, 100f), Tooltip("Sets the white balance to a custom color temperature.")]
            public float temperature;

            [Range(-100f, 100f), Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
            public float tint;

            #endregion Public Fields

            #region Public Properties

            public static BasicSettings defaultSettings
            {
                get
                {
                    return new BasicSettings
                    {
                        postExposure = 0f,

                        temperature = 0f,
                        tint = 0f,

                        hueShift = 0f,
                        saturation = 1f,
                        contrast = 1f,
                    };
                }
            }

            #endregion Public Properties
        }

        [Serializable]
        public struct ChannelMixerSettings
        {
            #region Public Fields

            public Vector3 blue;

            [HideInInspector]
            public int currentEditingChannel;

            public Vector3 green;
            public Vector3 red;

            #endregion Public Fields

            // Used only in the editor

            #region Public Properties

            public static ChannelMixerSettings defaultSettings
            {
                get
                {
                    return new ChannelMixerSettings
                    {
                        red = new Vector3(1f, 0f, 0f),
                        green = new Vector3(0f, 1f, 0f),
                        blue = new Vector3(0f, 0f, 1f),
                        currentEditingChannel = 0
                    };
                }
            }

            #endregion Public Properties
        }

        [Serializable]
        public struct ColorWheelsSettings
        {
            #region Public Fields

            [TrackballGroup]
            public LinearWheelsSettings linear;

            [TrackballGroup]
            public LogWheelsSettings log;

            public ColorWheelMode mode;

            #endregion Public Fields

            #region Public Properties

            public static ColorWheelsSettings defaultSettings
            {
                get
                {
                    return new ColorWheelsSettings
                    {
                        mode = ColorWheelMode.Log,
                        log = LogWheelsSettings.defaultSettings,
                        linear = LinearWheelsSettings.defaultSettings
                    };
                }
            }

            #endregion Public Properties
        }

        [Serializable]
        public struct CurvesSettings
        {
            #region Public Fields

            public ColorGradingCurve blue;

            // Used only in the editor
            [HideInInspector] public int e_CurrentEditingCurve;

            [HideInInspector] public bool e_CurveB;
            [HideInInspector] public bool e_CurveG;
            [HideInInspector] public bool e_CurveR;
            [HideInInspector] public bool e_CurveY;
            public ColorGradingCurve green;
            public ColorGradingCurve hueVShue;
            public ColorGradingCurve hueVSsat;
            public ColorGradingCurve lumVSsat;
            public ColorGradingCurve master;
            public ColorGradingCurve red;
            public ColorGradingCurve satVSsat;

            #endregion Public Fields

            #region Public Properties

            public static CurvesSettings defaultSettings
            {
                get
                {
                    return new CurvesSettings
                    {
                        master = new ColorGradingCurve(new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f)), 0f, false, new Vector2(0f, 1f)),
                        red = new ColorGradingCurve(new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f)), 0f, false, new Vector2(0f, 1f)),
                        green = new ColorGradingCurve(new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f)), 0f, false, new Vector2(0f, 1f)),
                        blue = new ColorGradingCurve(new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f)), 0f, false, new Vector2(0f, 1f)),

                        hueVShue = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
                        hueVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
                        satVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
                        lumVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),

                        e_CurrentEditingCurve = 0,
                        e_CurveY = true,
                        e_CurveR = false,
                        e_CurveG = false,
                        e_CurveB = false
                    };
                }
            }

            #endregion Public Properties
        }

        [Serializable]
        public struct LinearWheelsSettings
        {
            #region Public Fields

            [Trackball("GetGainValue")]
            public Color gain;

            [Trackball("GetGammaValue")]
            public Color gamma;

            [Trackball("GetLiftValue")]
            public Color lift;

            #endregion Public Fields

            #region Public Properties

            public static LinearWheelsSettings defaultSettings
            {
                get
                {
                    return new LinearWheelsSettings
                    {
                        lift = Color.clear,
                        gamma = Color.clear,
                        gain = Color.clear
                    };
                }
            }

            #endregion Public Properties
        }

        [Serializable]
        public struct LogWheelsSettings
        {
            #region Public Fields

            [Trackball("GetOffsetValue")]
            public Color offset;

            [Trackball("GetPowerValue")]
            public Color power;

            [Trackball("GetSlopeValue")]
            public Color slope;

            #endregion Public Fields

            #region Public Properties

            public static LogWheelsSettings defaultSettings
            {
                get
                {
                    return new LogWheelsSettings
                    {
                        slope = Color.clear,
                        power = Color.clear,
                        offset = Color.clear
                    };
                }
            }

            #endregion Public Properties
        }

        [Serializable]
        public struct Settings
        {
            #region Public Fields

            public BasicSettings basic;
            public ChannelMixerSettings channelMixer;
            public ColorWheelsSettings colorWheels;
            public CurvesSettings curves;
            public TonemappingSettings tonemapping;

            #endregion Public Fields

            #region Public Properties

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        tonemapping = TonemappingSettings.defaultSettings,
                        basic = BasicSettings.defaultSettings,
                        channelMixer = ChannelMixerSettings.defaultSettings,
                        colorWheels = ColorWheelsSettings.defaultSettings,
                        curves = CurvesSettings.defaultSettings
                    };
                }
            }

            #endregion Public Properties
        }

        [Serializable]
        public struct TonemappingSettings
        {
            #region Public Fields

            // Neutral settings
            [Range(-0.1f, 0.1f)]
            public float neutralBlackIn;

            [Range(-0.09f, 0.1f)]
            public float neutralBlackOut;

            [Range(1f, 10f)]
            public float neutralWhiteClip;

            [Range(1f, 20f)]
            public float neutralWhiteIn;

            [Range(0.1f, 20f)]
            public float neutralWhiteLevel;

            [Range(1f, 19f)]
            public float neutralWhiteOut;

            [Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use \"Neutral\" if you need a customizable tonemapper or \"Filmic\" to give a standard filmic look to your scenes.")]
            public Tonemapper tonemapper;

            #endregion Public Fields

            #region Public Properties

            public static TonemappingSettings defaultSettings
            {
                get
                {
                    return new TonemappingSettings
                    {
                        tonemapper = Tonemapper.Neutral,

                        neutralBlackIn = 0.02f,
                        neutralWhiteIn = 10f,
                        neutralBlackOut = 0f,
                        neutralWhiteOut = 10f,
                        neutralWhiteLevel = 5.3f,
                        neutralWhiteClip = 10f
                    };
                }
            }

            #endregion Public Properties
        }

        #endregion Public Structs
    }
}
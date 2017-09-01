using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class VignetteModel : PostProcessingModel
    {
        #region Private Fields

        [SerializeField]
        private Settings m_Settings = Settings.defaultSettings;

        #endregion Private Fields

        #region Public Enums

        public enum Mode
        {
            #region Public Fields

            Classic,
            Masked

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

            [Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
            public Vector2 center;

            [ColorUsage(false)]
            [Tooltip("Vignette color. Use the alpha channel for transparency.")]
            public Color color;

            [Range(0f, 1f), Tooltip("Amount of vignetting on screen.")]
            public float intensity;

            [Tooltip("A black and white mask to use as a vignette.")]
            public Texture mask;

            [Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
            public Mode mode;

            [Range(0f, 1f), Tooltip("Mask opacity.")]
            public float opacity;

            [Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
            public bool rounded;

            [Range(0f, 1f), Tooltip("Lower values will make a square-ish vignette.")]
            public float roundness;

            [Range(0.01f, 1f), Tooltip("Smoothness of the vignette borders.")]
            public float smoothness;

            #endregion Public Fields

            #region Public Properties

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        mode = Mode.Classic,
                        color = new Color(0f, 0f, 0f, 1f),
                        center = new Vector2(0.5f, 0.5f),
                        intensity = 0.45f,
                        smoothness = 0.2f,
                        roundness = 1f,
                        mask = null,
                        opacity = 1f,
                        rounded = false
                    };
                }
            }

            #endregion Public Properties
        }

        #endregion Public Structs
    }
}
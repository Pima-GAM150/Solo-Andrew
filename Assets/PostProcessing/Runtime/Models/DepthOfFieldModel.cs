using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class DepthOfFieldModel : PostProcessingModel
    {
        #region Private Fields

        [SerializeField]
        private Settings m_Settings = Settings.defaultSettings;

        #endregion Private Fields

        #region Public Enums

        public enum KernelSize
        {
            #region Public Fields

            Small,
            Medium

            #endregion Public Fields

,
            Large,
            VeryLarge
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

            [Range(0.05f, 32f), Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
            public float aperture;

            [Range(1f, 300f), Tooltip("Distance between the lens and the film. The larger the value is, the shallower the depth of field is.")]
            public float focalLength;

            [Min(0.1f), Tooltip("Distance to the point of focus.")]
            public float focusDistance;

            [Tooltip("Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).")]
            public KernelSize kernelSize;

            [Tooltip("Calculate the focal length automatically from the field-of-view value set on the camera. Using this setting isn't recommended.")]
            public bool useCameraFov;

            #endregion Public Fields

            #region Public Properties

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        focusDistance = 10f,
                        aperture = 5.6f,
                        focalLength = 50f,
                        useCameraFov = false,
                        kernelSize = KernelSize.Medium
                    };
                }
            }

            #endregion Public Properties
        }

        #endregion Public Structs
    }
}
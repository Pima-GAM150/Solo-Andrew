using System;

namespace UnityEngine.PostProcessing
{
    public class PostProcessingProfile : ScriptableObject
    {
#pragma warning disable 0169 // "field x is never used"

        public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();
        public AntialiasingModel antialiasing = new AntialiasingModel();
        public BloomModel bloom = new BloomModel();
        public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();
        public ColorGradingModel colorGrading = new ColorGradingModel();
        public BuiltinDebugViewsModel debugViews = new BuiltinDebugViewsModel();
        public DepthOfFieldModel depthOfField = new DepthOfFieldModel();
        public DitheringModel dithering = new DitheringModel();
        public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();
        public FogModel fog = new FogModel();
        public GrainModel grain = new GrainModel();
        public MonitorSettings monitors = new MonitorSettings();
        public MotionBlurModel motionBlur = new MotionBlurModel();
        public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();
        public UserLutModel userLut = new UserLutModel();
        public VignetteModel vignette = new VignetteModel();

#if UNITY_EDITOR

        // Monitor settings
        [Serializable]
        public class MonitorSettings
        {
            // Histogram
            public enum HistogramMode
            {
                Red = 0,
                Green = 1,
                Blue = 2,
                Luminance = 3,
                RGBMerged,
                RGBSplit
            }

            // Global
            public int currentMonitorID = 0;

            public HistogramMode histogramMode = HistogramMode.Luminance;

            // Callback used in the editor to grab the rendered frame and sent it to monitors
            public Action<RenderTexture> onFrameEndEditorOnly;

            // Parade
            public float paradeExposure = 0.12f;

            public bool refreshOnPlay = false;

            // Vectorscope
            public float vectorscopeExposure = 0.12f;

            public bool vectorscopeShowBackground = true;

            public bool waveformB = true;

            // Waveform
            public float waveformExposure = 0.12f;

            public bool waveformG = true;

            public bool waveformR = true;

            public bool waveformY = false;
        }

#endif
    }
}
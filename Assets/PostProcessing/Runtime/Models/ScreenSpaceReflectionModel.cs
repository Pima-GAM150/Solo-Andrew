using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class ScreenSpaceReflectionModel : PostProcessingModel
    {
        #region Private Fields

        [SerializeField]
        private Settings m_Settings = Settings.defaultSettings;

        #endregion Private Fields

        #region Public Enums

        public enum SSRReflectionBlendType
        {
            PhysicallyBased,
            Additive
        }

        public enum SSRResolution
        {
            High = 0,
            Low = 2
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
        public struct IntensitySettings
        {
            #region Public Fields

            [Tooltip("How far away from the maxDistance to begin fading SSR.")]
            [Range(0.0f, 1000.0f)]
            public float fadeDistance;

            [Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor.")]
            [Range(0.0f, 1.0f)]
            public float fresnelFade;

            [Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle.")]
            [Range(0.1f, 10.0f)]
            public float fresnelFadePower;

            [Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based.")]
            [Range(0.0f, 2.0f)]
            public float reflectionMultiplier;

            #endregion Public Fields
        }

        [Serializable]
        public struct ReflectionSettings
        {
            #region Public Fields

            // When enabled, we just add our reflections on top of the existing ones. This is physically incorrect, but several
            // popular demos and games have taken this approach, and it does hide some artifacts.
            [Tooltip("How the reflections are blended into the render.")]
            public SSRReflectionBlendType blendType;

            /// REFLECTIONS
            [Tooltip("Max raytracing length.")]
            [Range(16, 1024)]
            public int iterationCount;

            [Tooltip("Maximum reflection distance in world units.")]
            [Range(0.1f, 300.0f)]
            public float maxDistance;

            [Tooltip("Disable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")]
            public bool reflectBackfaces;

            [Tooltip("Blurriness of reflections.")]
            [Range(0.1f, 8.0f)]
            public float reflectionBlur;

            [Tooltip("Half resolution SSRR is much faster, but less accurate.")]
            public SSRResolution reflectionQuality;

            [Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes.")]
            [Range(1, 16)]
            public int stepSize;

            [Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind.")]
            [Range(0.01f, 10.0f)]
            public float widthModifier;

            #endregion Public Fields
        }

        [Serializable]
        public struct ScreenEdgeMask
        {
            #region Public Fields

            [Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion.")]
            [Range(0.0f, 1.0f)]
            public float intensity;

            #endregion Public Fields
        }

        [Serializable]
        public struct Settings
        {
            #region Public Fields

            public IntensitySettings intensity;
            public ReflectionSettings reflection;
            public ScreenEdgeMask screenEdgeMask;

            #endregion Public Fields

            #region Public Properties

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        reflection = new ReflectionSettings
                        {
                            blendType = SSRReflectionBlendType.PhysicallyBased,
                            reflectionQuality = SSRResolution.Low,
                            maxDistance = 100f,
                            iterationCount = 256,
                            stepSize = 3,
                            widthModifier = 0.5f,
                            reflectionBlur = 1f,
                            reflectBackfaces = false
                        },

                        intensity = new IntensitySettings
                        {
                            reflectionMultiplier = 1f,
                            fadeDistance = 100f,

                            fresnelFade = 1f,
                            fresnelFadePower = 1f,
                        },

                        screenEdgeMask = new ScreenEdgeMask
                        {
                            intensity = 0.03f
                        }
                    };
                }
            }

            #endregion Public Properties
        }

        #endregion Public Structs
    }
}
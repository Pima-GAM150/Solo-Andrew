namespace UnityEngine.PostProcessing
{
    public sealed class FxaaComponent : PostProcessingComponentRenderTexture<AntialiasingModel>
    {
        #region Public Properties

        public override bool active
        {
            get
            {
                return model.enabled
                       && model.settings.method == AntialiasingModel.Method.Fxaa
                       && !context.interrupted;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Render(RenderTexture source, RenderTexture destination)
        {
            var settings = model.settings.fxaaSettings;
            var material = context.materialFactory.Get("Hidden/Post FX/FXAA");
            var qualitySettings = AntialiasingModel.FxaaQualitySettings.presets[(int)settings.preset];
            var consoleSettings = AntialiasingModel.FxaaConsoleSettings.presets[(int)settings.preset];

            material.SetVector(Uniforms._QualitySettings,
                new Vector3(
                    qualitySettings.subpixelAliasingRemovalAmount,
                    qualitySettings.edgeDetectionThreshold,
                    qualitySettings.minimumRequiredLuminance
                    )
                );

            material.SetVector(Uniforms._ConsoleSettings,
                new Vector4(
                    consoleSettings.subpixelSpreadAmount,
                    consoleSettings.edgeSharpnessAmount,
                    consoleSettings.edgeDetectionThreshold,
                    consoleSettings.minimumRequiredLuminance
                    )
                );

            Graphics.Blit(source, destination, material, 0);
        }

        #endregion Public Methods

        #region Private Classes

        private static class Uniforms
        {
            #region Internal Fields

            internal static readonly int _ConsoleSettings = Shader.PropertyToID("_ConsoleSettings");
            internal static readonly int _QualitySettings = Shader.PropertyToID("_QualitySettings");

            #endregion Internal Fields
        }

        #endregion Private Classes
    }
}
namespace UnityEngine.PostProcessing
{
    public sealed class UserLutComponent : PostProcessingComponentRenderTexture<UserLutModel>
    {
        #region Public Properties

        public override bool active
        {
            get
            {
                var settings = model.settings;
                return model.enabled
                       && settings.lut != null
                       && settings.contribution > 0f
                       && settings.lut.height == (int)Mathf.Sqrt(settings.lut.width)
                       && !context.interrupted;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void OnGUI()
        {
            var settings = model.settings;
            var rect = new Rect(context.viewport.x * Screen.width + 8f, 8f, settings.lut.width, settings.lut.height);
            GUI.DrawTexture(rect, settings.lut);
        }

        public override void Prepare(Material uberMaterial)
        {
            var settings = model.settings;
            uberMaterial.EnableKeyword("USER_LUT");
            uberMaterial.SetTexture(Uniforms._UserLut, settings.lut);
            uberMaterial.SetVector(Uniforms._UserLut_Params, new Vector4(1f / settings.lut.width, 1f / settings.lut.height, settings.lut.height - 1f, settings.contribution));
        }

        #endregion Public Methods

        #region Private Classes

        private static class Uniforms
        {
            #region Internal Fields

            internal static readonly int _UserLut = Shader.PropertyToID("_UserLut");
            internal static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");

            #endregion Internal Fields
        }

        #endregion Private Classes
    }
}
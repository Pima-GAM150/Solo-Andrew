namespace UnityEngine.PostProcessing
{
    public sealed class DitheringComponent : PostProcessingComponentRenderTexture<DitheringModel>
    {
        #region Private Fields

        private const int k_TextureCount = 64;

        // Holds 64 64x64 Alpha8 textures (256kb total)
        private Texture2D[] noiseTextures;

        private int textureIndex = 0;

        #endregion Private Fields

        #region Public Properties

        public override bool active
        {
            get
            {
                return model.enabled
                       && !context.interrupted;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override void OnDisable()
        {
            noiseTextures = null;
        }

        public override void Prepare(Material uberMaterial)
        {
            float rndOffsetX;
            float rndOffsetY;

#if POSTFX_DEBUG_STATIC_DITHERING
            textureIndex = 0;
            rndOffsetX = 0f;
            rndOffsetY = 0f;
#else
            if (++textureIndex >= k_TextureCount)
                textureIndex = 0;

            rndOffsetX = Random.value;
            rndOffsetY = Random.value;
#endif

            if (noiseTextures == null)
                LoadNoiseTextures();

            var noiseTex = noiseTextures[textureIndex];

            uberMaterial.EnableKeyword("DITHERING");
            uberMaterial.SetTexture(Uniforms._DitheringTex, noiseTex);
            uberMaterial.SetVector(Uniforms._DitheringCoords, new Vector4(
                (float)context.width / (float)noiseTex.width,
                (float)context.height / (float)noiseTex.height,
                rndOffsetX,
                rndOffsetY
            ));
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadNoiseTextures()
        {
            noiseTextures = new Texture2D[k_TextureCount];

            for (int i = 0; i < k_TextureCount; i++)
                noiseTextures[i] = Resources.Load<Texture2D>("Bluenoise64/LDR_LLL1_" + i);
        }

        #endregion Private Methods

        #region Private Classes

        private static class Uniforms
        {
            #region Internal Fields

            internal static readonly int _DitheringCoords = Shader.PropertyToID("_DitheringCoords");
            internal static readonly int _DitheringTex = Shader.PropertyToID("_DitheringTex");

            #endregion Internal Fields
        }

        #endregion Private Classes
    }
}
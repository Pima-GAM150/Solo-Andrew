using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
    public sealed class MaterialFactory : IDisposable
    {
        #region Private Fields

        private Dictionary<string, Material> m_Materials;

        #endregion Private Fields

        #region Public Constructors

        public MaterialFactory()
        {
            m_Materials = new Dictionary<string, Material>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            var enumerator = m_Materials.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var material = enumerator.Current.Value;
                GraphicsUtils.Destroy(material);
            }

            m_Materials.Clear();
        }

        public Material Get(string shaderName)
        {
            Material material;

            if (!m_Materials.TryGetValue(shaderName, out material))
            {
                var shader = Shader.Find(shaderName);

                if (shader == null)
                    throw new ArgumentException(string.Format("Shader not found ({0})", shaderName));

                material = new Material(shader)
                {
                    name = string.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1)),
                    hideFlags = HideFlags.DontSave
                };

                m_Materials.Add(shaderName, material);
            }

            return material;
        }

        #endregion Public Methods
    }
}
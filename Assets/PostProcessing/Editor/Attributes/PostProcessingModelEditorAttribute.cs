using System;

namespace UnityEditor.PostProcessing
{
    public class PostProcessingModelEditorAttribute : Attribute
    {
        #region Public Fields

        public readonly bool alwaysEnabled;
        public readonly Type type;

        #endregion Public Fields

        #region Public Constructors

        public PostProcessingModelEditorAttribute(Type type, bool alwaysEnabled = false)
        {
            this.type = type;
            this.alwaysEnabled = alwaysEnabled;
        }

        #endregion Public Constructors
    }
}
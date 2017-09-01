namespace UnityEngine.PostProcessing
{
    public sealed class GetSetAttribute : PropertyAttribute
    {
        #region Public Fields

        public readonly string name;
        public bool dirty;

        #endregion Public Fields

        #region Public Constructors

        public GetSetAttribute(string name)
        {
            this.name = name;
        }

        #endregion Public Constructors
    }
}
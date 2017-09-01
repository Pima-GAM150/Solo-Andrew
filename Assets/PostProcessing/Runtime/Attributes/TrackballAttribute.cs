namespace UnityEngine.PostProcessing
{
    public sealed class TrackballAttribute : PropertyAttribute
    {
        #region Public Fields

        public readonly string method;

        #endregion Public Fields

        #region Public Constructors

        public TrackballAttribute(string method)
        {
            this.method = method;
        }

        #endregion Public Constructors
    }
}
namespace UnityEngine.PostProcessing
{
    public sealed class MinAttribute : PropertyAttribute
    {
        #region Public Fields

        public readonly float min;

        #endregion Public Fields

        #region Public Constructors

        public MinAttribute(float min)
        {
            this.min = min;
        }

        #endregion Public Constructors
    }
}
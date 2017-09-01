using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public abstract class PostProcessingModel
    {
        #region Private Fields

        [SerializeField, GetSet("enabled")]
        private bool m_Enabled;

        #endregion Private Fields

        #region Public Properties

        public bool enabled
        {
            get
            {
                return m_Enabled;
            }
            set
            {
                m_Enabled = value;

                if (value)
                    OnValidate();
            }
        }

        #endregion Public Properties

        #region Public Methods

        public virtual void OnValidate()
        { }

        public abstract void Reset();

        #endregion Public Methods
    }
}
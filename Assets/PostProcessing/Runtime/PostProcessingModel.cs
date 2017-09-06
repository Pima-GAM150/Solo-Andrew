using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public abstract class PostProcessingModel
    {
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

        [SerializeField, GetSet("enabled")]
        private bool m_Enabled;

        public virtual void OnValidate()
        { }

        public abstract void Reset();
    }
}
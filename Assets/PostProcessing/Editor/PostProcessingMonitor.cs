using System;
using UnityEngine;

namespace UnityEditor.PostProcessing
{
    using MonitorSettings = UnityEngine.PostProcessing.PostProcessingProfile.MonitorSettings;

    public abstract class PostProcessingMonitor : IDisposable
    {
        protected PostProcessingInspector m_BaseEditor;
        protected MonitorSettings m_MonitorSettings;

        public virtual void Dispose()
        { }

        public abstract GUIContent GetMonitorTitle();

        public void Init(MonitorSettings monitorSettings, PostProcessingInspector baseEditor)
        {
            m_MonitorSettings = monitorSettings;
            m_BaseEditor = baseEditor;
        }

        public abstract bool IsSupported();

        public virtual void OnFrameData(RenderTexture source)
        { }

        public abstract void OnMonitorGUI(Rect r);

        public virtual void OnMonitorSettings()
        { }
    }
}
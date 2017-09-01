using UnityEngine;

namespace UnityEditor.PostProcessing
{
    using UnityObject = Object;

    internal static class EditorResources
    {
        #region Private Fields

        private static string m_EditorResourcesPath = string.Empty;

        #endregion Private Fields

        #region Internal Properties

        internal static string editorResourcesPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_EditorResourcesPath))
                {
                    string path;

                    if (SearchForEditorResourcesPath(out path))
                        m_EditorResourcesPath = path;
                    else
                        Debug.LogError("Unable to locate editor resources. Make sure the PostProcessing package has been installed correctly.");
                }

                return m_EditorResourcesPath;
            }
        }

        #endregion Internal Properties

        #region Internal Methods

        internal static T Load<T>(string name)
            where T : UnityObject
        {
            return AssetDatabase.LoadAssetAtPath<T>(editorResourcesPath + name);
        }

        #endregion Internal Methods

        #region Private Methods

        private static bool SearchForEditorResourcesPath(out string path)
        {
            path = string.Empty;

            string searchStr = "/PostProcessing/Editor Resources/";
            string str = null;

            foreach (var assetPath in AssetDatabase.GetAllAssetPaths())
            {
                if (assetPath.Contains(searchStr))
                {
                    str = assetPath;
                    break;
                }
            }

            if (str == null)
                return false;

            path = str.Substring(0, str.LastIndexOf(searchStr) + searchStr.Length);
            return true;
        }

        #endregion Private Methods
    }
}
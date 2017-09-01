using System.IO;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace UnityEditor.PostProcessing
{
    public class PostProcessingFactory
    {
        #region Internal Methods

        internal static PostProcessingProfile CreatePostProcessingProfileAtPath(string path)
        {
            var profile = ScriptableObject.CreateInstance<PostProcessingProfile>();
            profile.name = Path.GetFileName(path);
            profile.fog.enabled = true;
            AssetDatabase.CreateAsset(profile, path);
            return profile;
        }

        #endregion Internal Methods

        #region Private Methods

        [MenuItem("Assets/Create/Post-Processing Profile", priority = 201)]
        private static void MenuCreatePostProcessingProfile()
        {
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreatePostProcessingProfile>(), "New Post-Processing Profile.asset", icon, null);
        }

        #endregion Private Methods
    }

    internal class DoCreatePostProcessingProfile : EndNameEditAction
    {
        #region Public Methods

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            PostProcessingProfile profile = PostProcessingFactory.CreatePostProcessingProfileAtPath(pathName);
            ProjectWindowUtil.ShowCreatedAsset(profile);
        }

        #endregion Public Methods
    }
}
using UnityEngine;

namespace EraseGame
{
    public static class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Gets the direction from this vector to b
        /// </summary>
        /// <param name="a">this</param>
        /// <param name="b">target</param>
        /// <returns>normalized Direction Vector</returns>
        public static Vector2 GetDirection(this Vector2 a, Vector2 b)
        {
            var heading = b - a;
            return heading / heading.magnitude;
        }

        #endregion Public Methods
    }
}
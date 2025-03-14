using UnityEngine;

namespace UnityUtilities.General
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Convert this vector2 into a flat Vector3 properly.
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }

        /// <summary>
        /// Conver tthis vector2 into a vector3 with a given y value.
        /// </summary>
        /// <param name="vector2"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector2 vector2, float y)
        {
            return new Vector3(vector2.x, y, vector2.y);
        }
    }
}
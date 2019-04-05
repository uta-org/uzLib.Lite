using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using uzLib.Lite.Extensions;

using Random = UnityEngine.Random;

namespace uzLib.Lite.Unity.Extensions
{
    /// <summary>
    /// The GeometryHelper class
    /// </summary>
    public static class GeometryHelper
    {
        /// <summary>
        /// The stored orthogrpahic sizes
        /// </summary>
        private static Dictionary<string, float> ortSizes = new Dictionary<string, float>();

        /// <summary>
        /// Gets the random position.
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetRandomPosition(float rndThreshold = 10000f)
        {
            float x = Random.Range(-rndThreshold, rndThreshold);
            float y = Random.Range(-rndThreshold, rndThreshold);
            float z = Random.Range(-rndThreshold, rndThreshold);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Gets the average.
        /// </summary>
        /// <param name="vectors">The vectors.</param>
        /// <returns></returns>
        public static Vector3 GetAverage(this IEnumerable<Vector3> vectors)
        {
            int count = 0;
            float sumX = 0, sumY = 0, sumZ = 0;

            foreach (Vector3 v in vectors)
            {
                sumX += v.x;
                sumY += v.y;
                sumZ += v.z;

                ++count;
            }

            return new Vector3(sumX, sumY, sumZ) / count;
        }

        /// <summary>
        /// Gets the encapsulated bounds.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <param name="renderers">The renderers.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">GameObject has no Renderers!</exception>
        public static Bounds GetEncapsulatedBounds(this GameObject gameObject, Renderer[] renderers = null)
        {
            renderers = renderers ?? gameObject.GetComponentsInChildren<Renderer>();

            if (renderers.Length == 0)
                throw new Exception("GameObject has no Renderers!");

            Bounds targetBounds = new Bounds(gameObject.transform.position, Vector3.one);
            foreach (Renderer renderer in renderers)
            {
                targetBounds.Encapsulate(renderer.bounds);
            }

            return targetBounds;
        }

        /// <summary>
        /// Gets the fitting orthographic size in bounds.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="gameObject">The game object.</param>
        /// <param name="f_forceMax">if set to <c>true</c> [f force maximum].</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">GameObject has no Renderers!</exception>
        public static float GetFittingOrthographicSizeInBounds(float width, float height, GameObject gameObject, bool f_forceMax = false)
        {
            if (f_forceMax && ortSizes.ContainsKey(gameObject.name))
                return ortSizes.SafeGet(gameObject.name);

            var renderers = gameObject.GetComponentsInChildren<Renderer>();

            if (renderers.Length == 0)
                throw new Exception("GameObject has no Renderers!");

            Bounds targetBounds = GetEncapsulatedBounds(gameObject, renderers);

            float screenRatio = width / height;
            float targetRatio = targetBounds.size.x / targetBounds.size.y;

            // TODO: Why I need this padding?
            float padding = Mathf.Sqrt(Mathf.Sqrt(2));
            float finalOrt = 0;

            if (screenRatio >= targetRatio)
            {
                finalOrt = targetBounds.size.y / 2 * padding;
            }
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                finalOrt = targetBounds.size.y / 2 * differenceInSize * padding;
            }

            if (finalOrt > ortSizes.SafeGet(gameObject.name))
                ortSizes.AddOrSet(gameObject.name, finalOrt);

            return finalOrt;
        }

        /// <summary>
        /// Gets the model offset.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <returns></returns>
        public static Vector3 GetModelOffset(this GameObject gameObject)
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>();

            float maxX = renderers.Max(r => r.bounds.center.x);
            float minX = renderers.Min(r => r.bounds.center.x);
            float maxY = renderers.Max(r => r.bounds.center.y);
            float minY = renderers.Min(r => r.bounds.center.y);

            return new Vector3(GetPositiveDiff(maxX, minX), GetPositiveDiff(maxY, minY));
        }

        /// <summary>
        /// Gets the positive difference.
        /// </summary>
        /// <param name="f1">The f1.</param>
        /// <param name="f2">The f2.</param>
        /// <returns></returns>
        public static float GetPositiveDiff(float f1, float f2)
        {
            float af1 = Mathf.Abs(f1);
            float af2 = Mathf.Abs(f2);

            if (af1 > af2)
                return (af1 >= 0 ? 1 : -1) * af1 - af2;
            else
                return (af2 >= 0 ? 1 : -1) * af2 - af1;
        }

        /// <summary>
        /// Determines whether [is greater or equal] [the specified other].
        /// </summary>
        /// <param name="local">The local.</param>
        /// <param name="other">The other.</param>
        /// <returns>
        ///   <c>true</c> if [is greater or equal] [the specified other]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGreaterOrEqual(this Vector2 local, Vector2 other)
        {
            if (local.x >= other.x && local.y >= other.y)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determines whether [is lesser or equal] [the specified other].
        /// </summary>
        /// <param name="local">The local.</param>
        /// <param name="other">The other.</param>
        /// <returns>
        ///   <c>true</c> if [is lesser or equal] [the specified other]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLesserOrEqual(this Vector2 local, Vector2 other)
        {
            if (local.x <= other.x && local.y <= other.y)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determines whether the specified other is greater.
        /// </summary>
        /// <param name="local">The local.</param>
        /// <param name="other">The other.</param>
        /// <param name="orOperator">if set to <c>true</c> [or operator].</param>
        /// <returns>
        ///   <c>true</c> if the specified other is greater; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGreater(this Vector2 local, Vector2 other, bool orOperator = false)
        {
            if (orOperator)
            {
                if (local.x > other.x || local.y > other.y)
                    return true;
                else
                    return false;
            }
            else
            {
                if (local.x > other.x && local.y > other.y)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the specified other is lesser.
        /// </summary>
        /// <param name="local">The local.</param>
        /// <param name="other">The other.</param>
        /// <param name="orOperator">if set to <c>true</c> [or operator].</param>
        /// <returns>
        ///   <c>true</c> if the specified other is lesser; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLesser(this Vector2 local, Vector2 other, bool orOperator = false)
        {
            if (orOperator)
            {
                if (local.x < other.x || local.y < other.y)
                    return true;
                else
                    return false;
            }
            else
            {
                if (local.x < other.x && local.y < other.y)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// To the vec2 with x and z.
        /// </summary>
        /// <param name="vec3">The vec3.</param>
        /// <returns></returns>
        public static Vector2 ToVec2WithXAndZ(this Vector3 vec3)
        {
            return new Vector2(vec3.x, vec3.z);
        }

        /// <summary>
        /// Withes the x and z.
        /// </summary>
        /// <param name="vec3">The vec3.</param>
        /// <returns></returns>
        public static Vector3 WithXAndZ(this Vector3 vec3)
        {
            return new Vector3(vec3.x, 0f, vec3.z);
        }

        /// <summary>
        /// Inverteds the specified vec3.
        /// </summary>
        /// <param name="vec3">The vec3.</param>
        /// <returns></returns>
        public static Vector3 Inverted(this Vector3 vec3)
        {
            return new Vector3(1.0f / vec3.x, 1.0f / vec3.y, 1.0f / vec3.z);
        }

        /// <summary>
        /// Gets the ray from center.
        /// </summary>
        /// <param name="cam">The cam.</param>
        /// <returns></returns>
        public static Ray GetRayFromCenter(this Camera cam)
        {
            Vector3 viewportPos = new Vector3(0.5f, 0.5f, 0f);
            return cam.ViewportPointToRay(viewportPos);
        }

        /// <summary>
        /// Gizmoses the draw line from camera.
        /// </summary>
        public static void GizmosDrawLineFromCamera()
        {
            if (null == Camera.main)
                return;

            Ray ray = Camera.main.GetRayFromCenter();

            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * Camera.main.farClipPlane);
        }
    }
}
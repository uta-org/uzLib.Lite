using UnityEngine;

namespace uzLib.Lite.ExternalCode.Unity.Extensions
{
    public static class VectorHelper
    {
        #region "Sum & Rest"

        public static Vector2 SumX(this Vector2 v, float value)
        {
            return new Vector2(v.x + value, v.y);
        }

        public static Vector2 SumY(this Vector2 v, float value)
        {
            return new Vector2(v.x, v.y + value);
        }

        public static Vector2 SumAll(this Vector2 v, float value)
        {
            return new Vector2(v.x + value, v.y + value);
        }

        public static Vector2 RestX(this Vector2 v, float value)
        {
            return new Vector2(v.x - value, v.y);
        }

        public static Vector2 RestY(this Vector2 v, float value)
        {
            return new Vector2(v.x, v.y - value);
        }

        public static Vector2 RestAll(this Vector2 v, float value)
        {
            return new Vector2(v.x - value, v.y - value);
        }

        public static Vector3 SumX(this Vector3 v, float value)
        {
            return new Vector3(v.x + value, v.y, v.z);
        }

        public static Vector3 SumY(this Vector3 v, float value)
        {
            return new Vector3(v.x, v.y + value, v.z);
        }

        public static Vector3 SumZ(this Vector3 v, float value)
        {
            return new Vector3(v.x, v.y, v.z + value);
        }

        public static Vector3 SumAll(this Vector3 v, float value)
        {
            return new Vector3(v.x + value, v.y + value, v.z + value);
        }

        public static Vector3 RestX(this Vector3 v, float value)
        {
            return new Vector3(v.x - value, v.y, v.z);
        }

        public static Vector3 RestY(this Vector3 v, float value)
        {
            return new Vector3(v.x, v.y - value, v.z);
        }

        public static Vector3 RestZ(this Vector3 v, float value)
        {
            return new Vector3(v.x, v.y, v.z - value);
        }

        public static Vector3 RestAll(this Vector3 v, float value)
        {
            return new Vector3(v.x - value, v.y - value, v.z - value);
        }

        public static Vector4 SumX(this Vector4 v, float value)
        {
            return new Vector4(v.x + value, v.y, v.z, v.w);
        }

        public static Vector4 SumY(this Vector4 v, float value)
        {
            return new Vector4(v.x, v.y + value, v.z, v.w);
        }

        public static Vector4 SumZ(this Vector4 v, float value)
        {
            return new Vector4(v.x, v.y, v.z + value, v.w);
        }

        public static Vector4 SumW(this Vector4 v, float value)
        {
            return new Vector4(v.x, v.y, v.z, v.w + value);
        }

        public static Vector4 SumAll(this Vector4 v, float value)
        {
            return new Vector4(v.x + value, v.y + value, v.z + value, v.w + value);
        }

        public static Vector4 RestX(this Vector4 v, float value)
        {
            return new Vector4(v.x - value, v.y, v.z, v.w);
        }

        public static Vector4 RestY(this Vector4 v, float value)
        {
            return new Vector4(v.x, v.y - value, v.z, v.w);
        }

        public static Vector4 RestZ(this Vector4 v, float value)
        {
            return new Vector4(v.x, v.y, v.z - value, v.w);
        }

        public static Vector4 RestW(this Vector4 v, float value)
        {
            return new Vector4(v.x, v.y, v.z, v.w - value);
        }

        public static Vector4 RestAll(this Vector4 v, float value)
        {
            return new Vector4(v.x - value, v.y - value, v.z - value, v.w - value);
        }

        #endregion "Sum & Rest"

        #region "Multiply & Divide"

        public static Vector2 MultiplyX(this Vector2 v, float value)
        {
            return new Vector2(v.x * value, v.y);
        }

        public static Vector2 MultiplyY(this Vector2 v, float value)
        {
            return new Vector2(v.x, v.y * value);
        }

        public static Vector2 MultiplyAll(this Vector2 v, float value)
        {
            return new Vector2(v.x * value, v.y * value);
        }

        public static Vector2 DivideX(this Vector2 v, float value)
        {
            return new Vector2(v.x / value, v.y);
        }

        public static Vector2 DivideY(this Vector2 v, float value)
        {
            return new Vector2(v.x, v.y / value);
        }

        public static Vector2 DivideAll(this Vector2 v, float value)
        {
            return new Vector2(v.x / value, v.y / value);
        }

        public static Vector3 MultiplyX(this Vector3 v, float value)
        {
            return new Vector3(v.x * value, v.y, v.z);
        }

        public static Vector3 MultiplyY(this Vector3 v, float value)
        {
            return new Vector3(v.x, v.y * value, v.z);
        }

        public static Vector3 MultiplyZ(this Vector3 v, float value)
        {
            return new Vector3(v.x, v.y, v.z * value);
        }

        public static Vector3 MultiplyAll(this Vector3 v, float value)
        {
            return new Vector3(v.x * value, v.y * value, v.z * value);
        }

        public static Vector3 DivideX(this Vector3 v, float value)
        {
            return new Vector3(v.x / value, v.y, v.z);
        }

        public static Vector3 DivideY(this Vector3 v, float value)
        {
            return new Vector3(v.x, v.y / value, v.z);
        }

        public static Vector3 DivideZ(this Vector3 v, float value)
        {
            return new Vector3(v.x, v.y, v.z / value);
        }

        public static Vector3 DivideAll(this Vector3 v, float value)
        {
            return new Vector3(v.x / value, v.y / value, v.z / value);
        }

        public static Vector4 MultiplyX(this Vector4 v, float value)
        {
            return new Vector4(v.x * value, v.y, v.z, v.w);
        }

        public static Vector4 MultiplyY(this Vector4 v, float value)
        {
            return new Vector4(v.x, v.y * value, v.z, v.w);
        }

        public static Vector4 MultiplyZ(this Vector4 v, float value)
        {
            return new Vector4(v.x, v.y, v.z * value, v.w);
        }

        public static Vector4 MultiplyW(this Vector4 v, float value)
        {
            return new Vector4(v.x, v.y, v.z, v.w * value);
        }

        public static Vector4 MultiplyAll(this Vector4 v, float value)
        {
            return new Vector4(v.x * value, v.y * value, v.z * value, v.w * value);
        }

        public static Vector4 DivideX(this Vector4 v, float value)
        {
            return new Vector4(v.x / value, v.y, v.z, v.w);
        }

        public static Vector4 DivideY(this Vector4 v, float value)
        {
            return new Vector4(v.x, v.y / value, v.z, v.w);
        }

        public static Vector4 DivideZ(this Vector4 v, float value)
        {
            return new Vector4(v.x, v.y, v.z / value, v.w);
        }

        public static Vector4 DivideW(this Vector4 v, float value)
        {
            return new Vector4(v.x, v.y, v.z, v.w / value);
        }

        public static Vector4 DivideAll(this Vector4 v, float value)
        {
            return new Vector4(v.x / value, v.y / value, v.z / value, v.w / value);
        }

        #endregion "Multiply & Divide"
    }
}
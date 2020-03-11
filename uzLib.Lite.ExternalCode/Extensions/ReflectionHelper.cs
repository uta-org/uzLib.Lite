using System;
using System.Diagnostics;
using System.Linq;

#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

using System.Drawing;
using Console = Colorful.Console;

#else

using Debug = UnityEngine.Debug;

#endif

using System.Reflection;

namespace uzLib.Lite.ExternalCode.Extensions
{
    //

    /// <summary>
    /// The ReflectionHelper class
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <param name="asm">The asm.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        public static void InvokeStaticMethod(this Assembly asm, string className, string methodName, params object[] parameters)
        {
            asm?.GetType(className)?.GetMethod(methodName)?.Invoke(null, parameters);
        }

        /// <summary>
        /// Runs the exception safe.
        /// </summary>
        /// <param name="function">The function.</param>
        public static void RunExceptionSafe(Action function)
        {
            try
            {
                function();
            }
            catch (Exception ex)
            {
#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5
                Console.WriteLine(ex, Color.Red);
#else
                Debug.LogException(ex);
#endif
            }
        }

        /// <summary>
        /// Invokes the specified method name.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="args">The arguments.</param>
        public static void Invoke(this object obj, string methodName, params object[] args)
        {
            var method = obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method != null)
            {
                method.Invoke(obj, args);
            }
        }

        /// <summary>
        /// Invokes the exception safe.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="args">The arguments.</param>
        public static void InvokeExceptionSafe(this object obj, string methodName, params object[] args)
        {
            try
            {
                obj.Invoke(methodName, args);
            }
            catch (Exception ex)
            {
#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5
                Console.WriteLine(ex, Color.Red);
#else
                Debug.LogException(ex);
#endif
            }
        }

        /// <summary>
        /// Determines whether the specified class name has method.
        /// </summary>
        /// <param name="asm">The asm.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>
        ///   <c>true</c> if the specified class name has method; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasMethod(this Assembly asm, string className, string methodName)
        {
            return asm.GetType(className).GetMethod(methodName) != null;
        }

        /// <summary>
        /// Gets the attribute from calling method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAttributeFromCallingMethod<T>()
            where T : Attribute
        {
            return (T)new StackTrace()
                .GetFrame(1)
                .GetMethod()
                .GetCustomAttributes(false)
                .FirstOrDefault(attr => attr.GetType() == typeof(T));
        }

        /// <summary>
        /// Gets the field information.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        private static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            FieldInfo fieldInfo;
            do
            {
                fieldInfo = type.GetField(fieldName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            }
            while (fieldInfo == null && type != null);
            return fieldInfo;
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">obj</exception>
        /// <exception cref="ArgumentOutOfRangeException">fieldName - @"Couldn't find field {fieldName} in type {objType.FullName}");</exception>
        public static object GetFieldValue(this object obj, string fieldName)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            Type objType = obj.GetType();
            FieldInfo fieldInfo = GetFieldInfo(objType, fieldName);
            if (fieldInfo == null)
                throw new ArgumentOutOfRangeException(nameof(fieldName),
                    $@"Couldn't find field {fieldName} in type {objType.FullName}");
            return fieldInfo.GetValue(obj);
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="val">The value.</param>
        /// <exception cref="ArgumentNullException">obj</exception>
        /// <exception cref="ArgumentOutOfRangeException">fieldName - @"Couldn't find field {fieldName} in type {objType.FullName}");</exception>
        public static void SetFieldValue(this object obj, string fieldName, object val)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            Type objType = obj.GetType();
            FieldInfo fieldInfo = GetFieldInfo(objType, fieldName);
            if (fieldInfo == null)
                throw new ArgumentOutOfRangeException(nameof(fieldName),
                    $@"Couldn't find field {fieldName} in type {objType.FullName}");
            fieldInfo.SetValue(obj, val);
        }
    }
}
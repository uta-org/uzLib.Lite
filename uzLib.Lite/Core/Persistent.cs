using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Extensions;
using uzLib.Lite.Extensions;

namespace UnityEngine.Core
{
    /// <summary>
    ///     THe Persistent class (a class to store generic values)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Persistent<T>
    {
        /// <summary>
        ///     The dictionary
        /// </summary>
        private static List<Persistent<T>> m_List = new List<Persistent<T>>();

        /// <summary>
        ///     The data value
        /// </summary>
        private T m_dataValue;

        /// <summary>
        ///     Is deserializing?
        /// </summary>
        private bool m_deseriazing = true;

        /// <summary>
        ///     Prevents a default instance of the <see cref="Persistent{T}" /> class from being created.
        /// </summary>
        private Persistent()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Persistent{T}" /> class.
        /// </summary>
        /// <param name="index">The index.</param>
        public Persistent(int index)
        {
            Index = index;
            // m_deseriazing = false;
        }

        /// <summary>
        ///     Gets the name of the friendly type.
        /// </summary>
        /// <value>
        ///     The name of the friendly type.
        /// </value>
        private static string FriendlyTypeName => typeof(T).GetFriendlyTypeName(true);

        /// <summary>
        ///     Gets the persistent path.
        /// </summary>
        /// <value>
        ///     The persistent path.
        /// </value>
        private static string PersistentPath => Path.Combine(Application.persistentDataPath,
            $"PersistentData-{FriendlyTypeName.GetValidFileName()}.json");

        /// <summary>
        ///     The index
        /// </summary>
        public int Index { get; }

        /// <summary>
        ///     The data
        /// </summary>
        public T Data
        {
            get => m_dataValue;
            set
            {
                // Debug.Log($"[{FriendlyTypeName}{(m_deseriazing ? " -- LOADING" : string.Empty)}] Persisted value: {value}");

                if (m_dataValue != null && !m_dataValue.Equals(value) || IsNull())
                {
                    m_dataValue = value;

                    // We don't need to persist if we are deserializing the instance
                    if (!m_deseriazing)
                        Persist();
                    else
                        m_deseriazing = false;
                }
            }
        }

        /// <summary>
        ///     Determines whether this instance is null.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance is null; otherwise, <c>false</c>.
        /// </returns>
        private bool IsNull()
        {
            return m_dataValue == null && typeof(T).IsValueType;
        }

        /// <summary>
        ///     Initializes the specified actions (forcing?).
        /// </summary>
        /// <param name="actions">The actions.</param>
        public static void Init(params Action<Persistent<T>>[] actions)
        {
            var fileExists = File.Exists(PersistentPath);
            // Debug.Log($"[{FriendlyTypeName} -- INIT] Persistent Data: {PersistentPath} || File Exists: {fileExists}");

            if (fileExists)
            {
                var contents = File.ReadAllText(PersistentPath);

                if (!string.IsNullOrEmpty(contents) && contents != "{}")
                    m_List = JsonConvert.DeserializeObject<List<Persistent<T>>>(contents);
                else
                    File.Delete(PersistentPath);
            }

            actions.ForEach((a, i) => a?.Invoke(GetFromList(i)));
        }

        /// <summary>
        ///     Gets from list.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private static Persistent<T> GetFromList(int index)
        {
            var element = m_List?.FirstOrDefault(e => e.Index == index);
            if (element == null)
            {
                var newElement = new Persistent<T>(index);

                m_List.Add(newElement);
                return newElement;
            }

            return m_List[index];
        }

        /// <summary>
        ///     Persists this instance.
        /// </summary>
        private void Persist()
        {
            try
            {
                // Update reference
                m_List[Index] = this;

                // Note: Json is separated from call to easily debug it
                var json = JsonHelper.JsonPrettify(JsonConvert.SerializeObject(m_List));
                {
                    // Debug.Log($"Persisting on '{PersistentPath}' || Data:\n{json}");
                }
                File.WriteAllText(PersistentPath, json);
            }
            catch
            // (Exception ex)
            {
                // This can be triggered in some cases when the constructor is initialized (I commented out the code -- Line 94)
                // Debug.LogException(ex);
            }
        }

        /// <summary>
        ///     Check if the persistent file exists.
        /// </summary>
        /// <returns></returns>
        public static bool Exists()
        {
            return File.Exists(PersistentPath);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Extensions
{
    /// <summary>
    ///     The Tuple Helper
    /// </summary>
    public static class TupleHelper
    {
        /// <summary>
        ///     Creates the string tuple.
        /// </summary>
        /// <param name="keyvaluePairs">The keyvalue pairs.</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, string>> CreateStringTuple(params string[] keyvaluePairs)
        {
            string key = "",
                value = "";
            var isKeySetted = false;

            for (var i = 0; i < keyvaluePairs.Length; ++i)
            {
                var str = keyvaluePairs[i];

                if (i % 2 == 0)
                {
                    if (isKeySetted)
                    {
                        yield return new Tuple<string, string>(key, value);
                        isKeySetted = false;
                    }

                    key = str;
                }
                else
                {
                    value = str;
                    isKeySetted = true;
                }
            }
        }

        /// <summary>
        ///     Creates the string tuple as array.
        /// </summary>
        /// <param name="keyvaluePairs">The keyvalue pairs.</param>
        /// <returns></returns>
        public static Tuple<string, string>[] CreateStringTupleAsArray(params string[] keyvaluePairs)
        {
            return CreateStringTuple(keyvaluePairs).ToArray();
        }
    }
}
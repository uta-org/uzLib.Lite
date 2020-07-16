using System;
using System.Collections.Generic;
using System.Threading;
using uzLib.Lite.ExternalCode.Extensions;

#if !(!UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5)

using uzLib.Lite.Extensions;

#endif

namespace uzLib.Lite.ExternalCode.Threading
{
    public class ThreadMarker : IDisposable
    {
        //[ThreadStatic]
        //private static string __Name = $"Unity Thread #{Thread.CurrentThread.ManagedThreadId}";

        private static readonly Dictionary<int, string> ThreadNames = new Dictionary<int, string>();

        public ThreadMarker(string name)
        {
            lock (ThreadNames)
            {
                ThreadNames.AddOrSet(Thread.CurrentThread.ManagedThreadId, name);
            }

            // __Name = name;
        }

        public static string Name
        {
            get
            {
                lock (ThreadNames)
                {
                    try
                    {
                        return ThreadNames[Thread.CurrentThread.ManagedThreadId];
                    }
                    catch
                    {
                        return $"Unity Thread #{Thread.CurrentThread.ManagedThreadId}";
                    }
                }
            }
        }

        public void Dispose()
        {
            ThreadNames.Remove(Thread.CurrentThread.ManagedThreadId);
            // __Name = "Un-Owned";
        }
    }
}
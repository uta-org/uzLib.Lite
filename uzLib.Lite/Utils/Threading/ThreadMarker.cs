using System;
using System.Collections.Generic;
using System.Threading;
using uzLib.Lite.Extensions;

namespace uzLib.Lite.Threading
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
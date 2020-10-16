//using System;
//using System.Collections;
//using System.Diagnostics;
//using CielaSpike;
//using uzLib.Lite.Extensions;
//using uzLib.Lite.Threading;

namespace UnityEngine.Utils.DebugTools
{
    //public static class ThreadedDebug
    //{
    //    private static double TimeRunning => (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;
    //    private static string FormattedTime => $"{TimeRunning.ConvertSecondsToDate()} ({TimeRunning:F2} s)";

    //    public static void Log(object obj, bool jumpBack = true)
    //    {
    //        Debug.Log($"[{ThreadMarker.Name} | {FormattedTime}] " + obj);
    //    }

    //    public static void LogFormat(string str, params object[] objs)
    //    {
    //        LogFormat($"[{ThreadMarker.Name} | {FormattedTime}] " + str, true, objs);
    //    }

    //    public static void LogFormat(string str, bool jumpBack = true, params object[] objs)
    //    {
    //        Debug.LogFormat($"[{ThreadMarker.Name} | {FormattedTime}] " + str, objs);
    //    }

    //    public static void LogWarning(object obj, bool jumpBack = true)
    //    {
    //        Debug.LogWarning($"[{ThreadMarker.Name} | {FormattedTime}] " + obj);
    //    }

    //    public static void LogWarningFormat(string str, params object[] objs)
    //    {
    //        LogWarningFormat($"[{ThreadMarker.Name} | {FormattedTime}] " + str, true, objs);
    //    }

    //    public static void LogWarningFormat(string str, bool jumpBack = true, params object[] objs)
    //    {
    //        Debug.LogWarningFormat($"[{ThreadMarker.Name} | {FormattedTime}] " + str, objs);
    //    }

    //    public static void LogError(object obj, bool jumpBack = true)
    //    {
    //        Debug.LogError($"[{ThreadMarker.Name} | {FormattedTime}] " + obj);
    //    }

    //    public static void LogErrorFormat(string str, params object[] objs)
    //    {
    //        LogErrorFormat($"[{ThreadMarker.Name} | {FormattedTime}] " + str, true, objs);
    //    }

    //    public static void LogErrorFormat(string str, bool jumpBack = true, params object[] objs)
    //    {
    //        Debug.LogErrorFormat($"[{ThreadMarker.Name} | {FormattedTime}] " + str, objs);
    //    }

    //    public static void LogException(Exception ex, bool jumpBack = true)
    //    {
    //        Debug.LogException(ex);
    //    }

    //    public static IEnumerator InspectTask(this Task task, int maxFrames = 500)
    //    {
    //        yield return Ninja.JumpToUnity;
    //        Log("Starting to inspect task!");
    //        yield return Ninja.JumpBack;

    //        var curFrame = 0;
    //        while (curFrame < maxFrames)
    //        {
    //            Log(task);
    //            yield return new WaitForEndOfFrame();

    //            ++curFrame;
    //        }
    //    }
    //}
}
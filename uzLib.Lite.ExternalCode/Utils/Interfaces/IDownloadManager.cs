using System;

namespace uzLib.Lite.ExternalCode.Utils.Interfaces
{
    public interface IDownloadManager<out T>
        where T : new()
        //where TResult : new()
    {
        T Target { get; }

        event Action<ulong, ulong> DownloadProgressChanged;

        event Action<object, byte[]> DownloadCompleted;

        void UnloadEvents();

        void DownloadDataAsync(object fromWhere);
    }
}
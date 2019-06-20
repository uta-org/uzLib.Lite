namespace uzLib.Lite.Utils
{
    public enum BatcherState
    {
        NoPendingDownloads,
        DownloadingItems,
        PendingAsyncDownloads,
        CreatedWebClient
    }
}
namespace uzLib.Lite.ExternalCode.Utils
{
    public enum BatcherState
    {
        NoPendingDownloads,
        DownloadingItems,
        PendingAsyncDownloads,
        CreatedWebClient,
        Exception
    }
}
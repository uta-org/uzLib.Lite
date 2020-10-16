using uzLib.Lite.ExternalCode.Utils.Interfaces;

namespace UnityEngine.Utils.Interfaces
{
    public interface IDownloadItem<T>
        where T : IFileModel
    {
        T FileModel { get; set; }
    }
}
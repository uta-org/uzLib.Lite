using uzLib.Lite.ExternalCode.Utils.Interfaces;

namespace uzLib.Lite.ExternalCode.Utils.Model
{
    /// <summary>
    /// This is an example of file model
    /// </summary>
    public class FileModel : IFileModel
    {
        public string FileUrl { get; }
        public long FileSize { get; }

        private FileModel() { }

        public FileModel(string fileUrl, long fileSize)
        {
            FileUrl = fileUrl;
            FileSize = fileSize;
        }
    }
}

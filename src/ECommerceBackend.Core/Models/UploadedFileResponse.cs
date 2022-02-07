namespace ECommerceBackend.Core.Models
{
    public class UploadedFileResponse
    {
        public string UploadedFileId { get; internal set; }
        public string Title { get; internal set; }
        public string FileName { get; internal set; }
        public string Name { get; internal set; }
        public string URL { get; internal set; }
        public string MediaUrl { get; internal set; }
        public string Thumbnail { get; internal set; }
        public string FilePath { get; internal set; }
        public string FileType { get; internal set; }
        public int Height { get; internal set; }
        public int Width { get; internal set; }
        public int Size { get; internal set; }
    }
}

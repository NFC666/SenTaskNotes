namespace SenNotes.Common.Models
{
    public class FileSource
    {
        public FileType FileType { get; set; }
        public string? FilePath { get; set; }

        public string? FileName => Path.GetFileName(FilePath);
    }
}
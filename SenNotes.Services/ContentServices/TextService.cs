using SenNotes.Services.IServices;

namespace SenNotes.Services.ContentServices
{
    public class TextService : IContentService
    {
        public string GetContent(string? filePath)
        {
            if (filePath == null)
                return string.Empty;
            string content = File.ReadAllText(filePath);
            return content;
        }
    }
}
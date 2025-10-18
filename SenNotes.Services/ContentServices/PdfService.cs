using System.Text;

using SenNotes.Services.IServices;

using UglyToad.PdfPig;

namespace SenNotes.Services
{
    public class PdfService : IContentService
    {
        public string GetContent(string? filePath)
        {
            if (filePath == null)
                return string.Empty;
            StringBuilder sb = new();
            using var document = PdfDocument.Open(filePath);
            foreach (var page in document.GetPages())
            {
                string text = page.Text;
                sb.Append(text);
            }
            return sb.ToString();
        }
    }
}
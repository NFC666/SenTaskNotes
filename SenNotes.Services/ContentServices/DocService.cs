using System.Text;

using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;

using SenNotes.Services.IServices;


namespace SenNotes.Services
{
    public class DocService : IContentService
    {
        public string GetContent(string? filePath)
        {
            if (filePath == null)
                return string.Empty;
            StringBuilder sb = new();
            using WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false);
            var body = wordDoc.MainDocumentPart?.Document.Body;
            if (body == null)
                return string.Empty;
            
            foreach (var element in body)
            {
                sb.Append(element.InnerText);
            }
            return sb.ToString();
        }
    }
}
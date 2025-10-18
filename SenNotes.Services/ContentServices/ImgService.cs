using SenNotes.Services.IServices;

using Tesseract;

namespace SenNotes.Services.ContentServices
{
    public class ImgService : IContentService
    {
        public string GetContent(string? filePath)
        {
            if (filePath == null)
                return string.Empty;
            using var engine = new TesseractEngine(@"./tessdata", "chi_sim", EngineMode.LstmOnly);
            using var img = Pix.LoadFromFile(filePath);
            using var page = engine.Process(img);
            return page.GetText();
        }
    }
}
using System.Net.Http.Json;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using SenNotes.Common.Global;
using SenNotes.Common.Helpers;
using SenNotes.Common.Models;
using SenNotes.Repository.Factories;
using SenNotes.Services.ContentServices;
using SenNotes.Services.IServices;

namespace SenNotes.Services
{
    //所有函数需要尽量保持无副作用，以便于检查
    public class AiAssistService
    {
        private readonly IContentService _textService = new TextService();
        private readonly IContentService _docxService = new DocService();
        private readonly IContentService _pdfService = new PdfService();
        private readonly IContentService _imgService = new ImgService();
        private readonly ISettingsModelService _settingsModelService 
            = new SettingsModelService();
        private readonly HttpClient _httpClient;

        private bool _isClientInfoSet = false;

        public AiAssistService()
        {
            _ = InitializeAsync();
            _httpClient = HttpFactory.GetHttpClient();
        }

        private async Task InitializeAsync()
        {
            var settings = await _settingsModelService.GetAllSettings();
            var setting = settings.FirstOrDefault();
            HttpFactory.ConfigClient(setting?.ApiKey,setting?.BaseUrl);
            _isClientInfoSet =  true;
            
        }

        /// <summary>
        /// 读取文件内容内容并分析
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<List<TaskModel>> 
            InCloudFromFileSourcesAndMessageAsync(List<FileSource>? filePath, string message)
        {
            List<Message> contents = SetContent(filePath, message);
            var payload = new ChatPayload
            {
                Model = "deepseek-ai/DeepSeek-V3.2-Exp",
                Messages = contents
            };
            var requestBody = JsonConvert.SerializeObject(payload);
            
            var resp = await _httpClient
                .PostAsJsonAsync("chat/completions", payload);
            var res = await resp.Content.ReadAsStringAsync();
            var strTask = JsonResponseHelper.GetValue<string>(res, "content");
            string pattern = @"(?s)\[.*?\]";
            Match match = Regex.Match(strTask, pattern);
            var tasks = JsonConvert.DeserializeObject<List<TaskModel>>(match.Value);
            if (tasks == null)
                return new List<TaskModel>();
            tasks.ForEach(task =>
            {
                task.InputMessage = message;
                task.Files = filePath;
            });
            return tasks;
            
        }
        // /// <summary>
        // /// 读取文本文件内容并分析
        // /// </summary>
        // /// <param name="filePath"></param>
        // /// <param name="message"></param>
        // /// <returns></returns>
        // /// <exception cref="Exception"></exception>
        // public async Task<string> InCloudFromFileSourcesAndMessageAsync(FileSource filePath,string message)
        // {
        //     if(filePath.FileType is not FileType.Text)
        //         throw new Exception("请传入txt文件");
        //     List<Message> contents = SetContent(filePath.FilePath, message, FileType.Text);
        //     var payload = new ChatPayload
        //     {
        //         Models = "deepseek-ai/DeepSeek-V3.2-Exp",
        //         Messages = contents
        //     };
        //     var requestBody = JsonConvert.SerializeObject(payload);
        //     
        //     var resp = await _httpClient
        //         .PostAsJsonAsync("chat/completions", payload);
        //     return await resp.Content.ReadAsStringAsync();
        //     
        // }
        //
        // public async Task<string> InCloudTaskFromDocxFIle(FileSource filePath, string message)
        // {
        //     if(filePath.FileType is not FileType.Doc)
        //         throw new Exception("请传入docx文件");
        //     List<Message> contents = SetContent(filePath.FilePath, message, FileType.Doc);
        //     var payload = new ChatPayload
        //     {
        //         Models = "deepseek-ai/DeepSeek-V3.2-Exp",
        //         Messages = contents
        //     };
        //     var requestBody = JsonConvert.SerializeObject(payload);
        //     
        //     var resp = await _httpClient
        //         .PostAsJsonAsync("chat/completions", payload);
        //     return await resp.Content.ReadAsStringAsync();
        // }
        //
        // public async Task<string> InCloudTaskFromImgFIle(FileSource filePath, string message)
        // {
        //     List<Message> contents = SetContent(filePath.FilePath, message, filePath.FileType);
        //     var payload = new ChatPayload
        //     {
        //         Models = "deepseek-ai/DeepSeek-V3.2-Exp",
        //         Messages = contents
        //     };
        //     var requestBody = JsonConvert.SerializeObject(payload);
        //     
        //     var resp = await _httpClient
        //         .PostAsJsonAsync("chat/completions", payload);
        //     return await resp.Content.ReadAsStringAsync();
        // }

        /// <summary>
        /// 获取文件内容并且由AI解析
        /// </summary>
        /// <param name="fileSource"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<string> SummarizeTasksFromFile(FileSource fileSource, string message)
        {
            List<Message> contents = SetContent(fileSource.FilePath, message, fileSource.FileType);
            var payload = new ChatPayload
            {
                Model = "deepseek-ai/DeepSeek-V3.2-Exp",
                Messages = contents
            };
            var requestBody = JsonConvert.SerializeObject(payload);
            
            var resp = await _httpClient
                .PostAsJsonAsync("chat/completions", payload);
            return await resp.Content.ReadAsStringAsync();
        }
        private List<Message> SetContent(List<FileSource>? filePath, string message)
        {
            List<Message> contents = new();
            contents.Add(new Message
            {
                Role = "user",
                Content = $"当前时间{DateTime.Now:yyyy-MM-dd HH:mm:ss}"
            });
            contents.Add(new Message
            {
                Role = "user",
                Content = message
            });
            contents.Add(new Message
            {
                Role = "system",
                Content = "请从以下文本中提取所有任务，并以 JSON 数组的形式返回。数组中每个元素是一个对象，包含以下字段：" +
                          "开始时间（start_time）、截止时间（end_time）、任务标题（task_title），任务内容（task_detail）。" +
                          "开始时间（start_time）、截止时间（end_time）的小时数必须是 0~23，出现在任何地方的小时数必须为0-23"+
                          "没有提及的时间或者其他主体可以null作为结果" +
                          "例如：[{\"start_time\": \"2025-10-01 12:00:00\", \"end_time\": \"2025-10-05 12:00:00\", \"task_title\": \"撰写报告\", \"task_detail\": \"报告要求不少于八百字...\"}]" +
                          "。请仅输出 JSON 数组，不要添加额外说明。"
            });
            if (filePath != null)
            {
                foreach (var file in filePath)
                {
                    switch (file.FileType)
                    {
                        case FileType.Text:
                            contents.Add(new Message
                            {
                                Role = "user",
                                Content = _textService.GetContent(file.FilePath)
                            });
                            break;
                        case FileType.Doc:
                            contents.Add(new Message
                            {
                                Role = "user",
                                Content = _docxService.GetContent(file.FilePath)
                            });
                            break;
                        case FileType.Pdf:
                            contents.Add(new Message
                            {
                                Role = "user",
                                Content = _pdfService.GetContent(file.FilePath)
                            });
                            break;
                        case FileType.Img:
                            contents.Add(new Message
                            {
                                Role = "user",
                                Content = _imgService.GetContent(file.FilePath)
                            });
                            break;
                    
                    }
                }
            }
            return contents;
        }
        private List<Message> SetContent(string? filePath, string message, FileType fileType)
        {
            List<Message> contents = new();
            contents.Add(new Message
            {
                Role = "user",
                Content = $"当前时间{DateTime.Now:yyyy-MM-dd HH:mm:ss}"
            });
            contents.Add(new Message
            {
                Role = "user",
                Content = message
            });
            contents.Add(new Message
            {
                Role = "system",
                Content = "请从以下文本中提取所有任务，并以 JSON 数组的形式返回。数组中每个元素是一个对象，包含以下字段：" +
                          "开始时间（start_time）、截止时间（end_time）、任务标题（task_title），任务内容（task_detail）。" +
                          "没有提及的时间或者其他主体可以null作为结果" +
                          "例如：[{\"start_time\": \"2025-10-01 12:00:00\", \"end_time\": \"2025-10-05 12:00:00\", \"task_title\": \"撰写报告\", \"task_detail\": \"报告要求不少于八百字...\"}]" +
                          "。请仅输出 JSON 数组，不要添加额外说明。"
            });
            switch (fileType)
            {
                case FileType.Text:
                    contents.Add(new Message
                    {
                        Role = "user",
                        Content = _textService.GetContent(filePath)
                    });
                    break;
                case FileType.Doc:
                    contents.Add(new Message
                    {
                        Role = "user",
                        Content = _docxService.GetContent(filePath)
                    });
                    break;
                case FileType.Img:
                    contents.Add(new Message
                    {
                        Role = "user",
                        Content = _imgService.GetContent(filePath)
                    });
                    break;
                case FileType.Pdf:
                    contents.Add(new Message
                    {
                        Role = "user",
                        Content = _pdfService.GetContent(filePath)
                    });
                    break;

            }
            return contents;
        }
    }
}
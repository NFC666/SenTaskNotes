using System.Net.Http.Headers;

namespace SenNotes.Repository.Factories
{
    public class HttpFactory
    {
        private static readonly HttpClient _httpClient = new();

        public static void ConfigClient(string? token, string? baseUrl)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(baseUrl))
            {
                return;
            }
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Authorization 
                = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// 获取HttpClient
        /// </summary>
        /// <param name="token"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static HttpClient GetHttpClient()
        {
            if(_httpClient.DefaultRequestHeaders.Authorization == null
               || _httpClient.BaseAddress == null)
                throw new Exception("请在软件左上角点击后设置Token和BaseUrl");
            return _httpClient;
        }
    }
}
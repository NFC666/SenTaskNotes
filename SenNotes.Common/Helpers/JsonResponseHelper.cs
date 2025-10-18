using Newtonsoft.Json.Linq;

namespace SenNotes.Common.Helpers
{
    public class JsonResponseHelper
    {
        public static T? GetValue<T>(string response, string key)
        {
            JObject json = JObject.Parse(response); 
            var type = typeof(T);
            if (!key.StartsWith("$"))
            {
                key = "$.." + key;
            }

            var tokens = json.SelectTokens(key).ToList();
            
            if(tokens.Count == 0)
                throw new Exception($"没有发现{key}");
            //对T是一般数组的处理
            if (type.IsArray)
            {
                var elementType = type.GetElementType();

                if(tokens.Count == 1 && tokens[0] is JArray jar)
                {
                    return jar.ToObject<T>();
                }
                var arr = Array.CreateInstance(elementType, tokens.Count);
                for (int i = 0; i < tokens.Count; i++)
                {
                    arr.SetValue(tokens[i].ToObject(elementType), i);
                }
                return (T)(object)arr;

            }
            //对T是基础类型的处理
            if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal))
                return tokens[0].Value<T>();
        
            //对T是一般对象的处理
            return tokens[0].ToObject<T>();
        }
    }
}
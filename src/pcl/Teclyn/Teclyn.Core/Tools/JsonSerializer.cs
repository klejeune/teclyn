using Newtonsoft.Json;

namespace Teclyn.Core.Tools
{
    public class JsonSerializer
    {
        public string Serialize(object @object)
        {
            return JsonConvert.SerializeObject(@object);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        
        public object Deserialize(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }
    }
}
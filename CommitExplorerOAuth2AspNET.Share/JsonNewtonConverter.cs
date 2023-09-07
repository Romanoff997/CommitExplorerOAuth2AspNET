using CommitExplorerOAuth2AspNET.Shared.Interface;
using Newtonsoft.Json;

namespace CommitExplorerOAuth2AspNET.Shared.Services
{
    public class JsonNewtonConverter: IJsonConverter
    {
        private  JsonSerializerSettings _settings { get; set; }
        public JsonNewtonConverter(JsonSerializerSettings settings )
        {
            _settings = settings;
        }

        public string WriteJson<T>(T value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }

        public T ReadJson<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, _settings);
        }
    }
}

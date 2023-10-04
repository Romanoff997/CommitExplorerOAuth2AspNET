using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitExplorerOAuth2AspNET.Service
{
    public class JsonNewtonConverter : IJsonConverter
    {
        private JsonSerializerSettings _settings { get; set; }
        public JsonNewtonConverter(JsonSerializerSettings settings)
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

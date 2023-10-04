using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitExplorerOAuth2AspNET.Service
{
    public interface IJsonConverter
    {
        public string WriteJson<T>(T value);

        public T ReadJson<T>(string value);
    }
}

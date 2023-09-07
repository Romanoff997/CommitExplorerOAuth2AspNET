namespace CommitExplorerOAuth2AspNET.Shared.Interface
{
    public interface IJsonConverter
    {
        public string WriteJson<T>(T value);

        public T ReadJson<T>(string value);
    }
}
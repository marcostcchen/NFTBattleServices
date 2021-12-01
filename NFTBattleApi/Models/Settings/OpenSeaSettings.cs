namespace NFTBattleApi.Settings
{
    public interface IOpenSeaSettings
    {
        string Url { get; set; }
    }
    public class OpenSeaSettings : IOpenSeaSettings
    {
        public string Url { get; set; }
    }
}

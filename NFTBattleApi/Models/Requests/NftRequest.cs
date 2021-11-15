namespace NFTBattleApi.Models
{
    public class NftRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Health { get; set; }
        public int? Attack { get; set; }
        public int? Defence { get; set; }
    }
}
namespace NFTBattleApi.Models.Entities
{
    public class NftAsset
    {
        public string token_id { get; set; }
        public string image_url { get; set; }
        public string name { get; set; }
        public dynamic asset_contract { get; set; }
        public string external_link { get; set; }
        public string owner { get; set; }
        public List<Traits> traits { get; set; }
    }

    public class Traits
    {
        public string trait_type { get; set; }
        public string value { get; set; }
        public string display_type { get; set; }
    }
}

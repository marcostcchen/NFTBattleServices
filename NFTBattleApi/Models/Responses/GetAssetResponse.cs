using NFTBattleApi.Models.Entities;

namespace NFTBattleApi.Models.Responses
{
    public class GetAssetResponse
    {
        public List<NftAsset> Assets { get; set; }
    }
    public class NftAsset
    {
        public string Token_id { get; set; }
        public string Image_url { get; set; }
        public string Name { get; set; }
        public string External_link { get; set; }
        public OpenSeaOwner? Owner { get; set; }
        public List<Traits> Traits { get; set; }
    }

    public class Traits
    {
        public string? Trait_type { get; set; }
        public dynamic? Value { get; set; }
        public string? Display_type { get; set; }
    }

    public class OpenSeaOwner
    {
        public OpenSeaUser User { get; set; }
        public string Profile_img_url { get; set; }
    }

    public class OpenSeaUser
    {
        public string Username { get; set; }
    }
}

using NFTBattleApi.Models.Entities;

namespace NFTBattleApi.Models
{
    public class BuyNftResponse
    {
        public User user { get; set; }
        public Nft nft { get; set; }
    }
}
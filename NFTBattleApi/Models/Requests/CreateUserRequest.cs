namespace NFTBattleApi.Models
{
    public class CreateUserRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string WalletId { get; set; }
    }
}

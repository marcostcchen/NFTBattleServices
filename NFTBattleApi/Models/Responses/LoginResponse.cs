namespace NFTBattleApi.Models
{
    public class LoginResponse
    {
        public string token { get; set; }
        public string type { get; set; }
        public User user { get; set; }
    }
}

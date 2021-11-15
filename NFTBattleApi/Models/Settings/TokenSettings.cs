namespace NFTBattleApi.Models
{

    public class TokenSettings: ITokenSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
    }

    public interface ITokenSettings
    {
        string JwtKey { get; set; }
        string JwtIssuer { get; set; }
    }
}

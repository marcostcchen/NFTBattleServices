using Microsoft.AspNetCore.Cors;
using NFTBattleApi.Models.Entities;
using NFTBattleApi.Models.Responses;
using NFTBattleApi.Settings;
using System.Text.Json;

namespace NFTBattleApi.Services
{
    public class OpenSeaService
    {
        private readonly IOpenSeaSettings _openSeaSettings;
        private readonly LogService _logService;
        public OpenSeaService(IOpenSeaSettings openSeaSettings, LogService logService)
        {
            _openSeaSettings = openSeaSettings;
            _logService = logService;
        }

        [EnableCors("_myAllowSpecificOrigins")]
        public async Task<List<Nft>> GetAssets()
        {
            using (var client = new HttpClient())
            {
                _logService.Info(new Log() { values = "Get Assets In"});
                client.Timeout = TimeSpan.FromSeconds(100);
                client.BaseAddress = new Uri(_openSeaSettings.Url);

                var parameters = "/assets?order_direction=desc&offset=0&limit=5&collection=ethermon";
                var result = await client.GetAsync(parameters).ConfigureAwait(false);

                if (!result.IsSuccessStatusCode) throw new Exception(result.ReasonPhrase);
                var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var nftAssets = JsonSerializer.Deserialize<GetAssetResponse>(content, serializerOptions)?.Assets ?? new List<NftAsset>();

                var nfts = new List<Nft>();

                nftAssets.ForEach(asset =>
                {
                    nfts.Add(new Nft()
                    {
                        Token_id = asset.Token_id,
                        Image_url = asset.Image_url,
                        Name = asset.Name,
                        Owner = null,
                        //Owner = new Owner()
                        //{
                        //    Name = asset.Owner?.User.Username
                        //},
                        Attack = new Random().Next(0, 100),
                        Defence = new Random().Next(0, 100),
                        Health = new Random().Next(0, 100),
                    });
                });

                return nfts;
            }
        }
    }
}

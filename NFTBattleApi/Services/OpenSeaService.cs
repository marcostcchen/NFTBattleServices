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
                _logService.Info(new Log() { values = "Get Assets In" });
                client.Timeout = TimeSpan.FromSeconds(100);
                client.BaseAddress = new Uri(_openSeaSettings.Url);

                var parameters = "/assets?order_direction=desc&offset=0&limit=20&collection=teste-bitmonster";
                var result = await client.GetAsync(parameters).ConfigureAwait(false);

                if (!result.IsSuccessStatusCode) throw new Exception(result.ReasonPhrase);
                var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var nftAssets = JsonSerializer.Deserialize<GetAssetResponse>(content, serializerOptions)?.Assets ?? new List<NftAsset>();

                var nfts = new List<Nft>();

                nftAssets.ForEach(asset =>
                {
                    var Attack = asset.Traits.Find(trait => trait.Trait_type == "Attack")?.Value?.GetInt32() ?? -1;
                    var Defence = asset.Traits.Find(trait => trait.Trait_type == "Defense")?.Value?.GetInt32() ?? -1;
                    var Vitality = asset.Traits.Find(trait => trait.Trait_type == "Vitality")?.Value?.GetInt32() ?? -1;

                    nfts.Add(new Nft()
                    {
                        Token_id = asset.Token_id,
                        Image_url = asset.Image_url,
                        Name = asset.Name,
                        Owner = null,
                        Attack = Attack,
                        Defence = Defence,
                        Health = Vitality,
                        PermaLink = asset.PermaLink,
                    });
                });

                return nfts;
            }
        }

        [EnableCors("_myAllowSpecificOrigins")]
        public async Task<List<Nft>> GetAssetsByOwner(string walletId)
        {
            using (var client = new HttpClient())
            {
                _logService.Info(new Log() { values = "Get Assets In" });
                client.Timeout = TimeSpan.FromSeconds(100);
                client.BaseAddress = new Uri(_openSeaSettings.Url);

                var parameters = $"/assets?owner={walletId}";
                var result = await client.GetAsync(parameters).ConfigureAwait(false);

                if (!result.IsSuccessStatusCode) throw new Exception();
                var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var nftAssets = JsonSerializer.Deserialize<GetAssetResponse>(content, serializerOptions)?.Assets ?? new List<NftAsset>();

                var nfts = new List<Nft>();


                nftAssets.ForEach(asset =>
                {
                    var Attack = asset.Traits.Find(trait => trait.Trait_type == "Attack")?.Value?.GetInt32() ?? -1;
                    var Defence = asset.Traits.Find(trait => trait.Trait_type == "Defense")?.Value?.GetInt32() ?? -1;
                    var Vitality = asset.Traits.Find(trait => trait.Trait_type == "Vitality")?.Value?.GetInt32() ?? -1;

                    nfts.Add(new Nft()
                    {
                        Token_id = asset.Token_id,
                        Image_url = asset.Image_url,
                        Name = asset.Name,
                        Owner = null,
                        Attack = Attack,
                        Defence = Defence,
                        Health = Vitality,
                        PermaLink = asset.PermaLink,
                    });
                });
                return nfts;
            }
        }
    }
}

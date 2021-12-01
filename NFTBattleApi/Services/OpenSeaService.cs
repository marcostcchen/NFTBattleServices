using NFTBattleApi.Models.Entities;
using NFTBattleApi.Settings;
using System.Text.Json;

namespace NFTBattleApi.Services
{
    public class OpenSeaService
    {
        private readonly OpenSeaSettings _openSeaSettings;
        public OpenSeaService(OpenSeaSettings openSeaSettings)
        {
            _openSeaSettings = openSeaSettings;
        }

        public async Task<List<NftAsset>> GetAssets()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_openSeaSettings.Url);
                var parameters = "assets?asset_contract_address=0xabc7e6c01237e8eef355bba2bf925a730b714d5f&order_direction=desc&offset=0&limit=50&collection=cryptosaga";
                var responseTask = client.GetAsync(parameters);

                var result = responseTask.Result;
                if (!result.IsSuccessStatusCode) throw new Exception("Erro do OpenSea");

                var content = await result.Content.ReadAsStringAsync();
                var nftAssets = JsonSerializer.Deserialize<List<NftAsset>>(content) ?? new List<NftAsset>();
                return nftAssets;
            }
        }
    }
}

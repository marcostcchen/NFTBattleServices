// See https://aka.ms/new-console-template for more information


using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json;

using (var client = new HttpClient())
{
    client.BaseAddress = new Uri("https://api.opensea.io/api/v1");

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

    Console.Write(JsonSerializer.Serialize(nfts));
}

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

public class Nft
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Token_id { get; set; }
    public string Image_url { get; set; }
    public string Name { get; set; }
    public Owner Owner { get; set; }
    public int Attack { get; set; }
    public int Defence { get; set; }
    public int Health { get; set; }
}

public class Owner
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
}
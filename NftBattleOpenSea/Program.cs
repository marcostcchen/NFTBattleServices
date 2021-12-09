using System.Text.Json;

//Funcao da chamada
async Task<List<Nft>> getUserNfts()
{
    using (var client = new HttpClient())
    {
        var parameters = "/usernft";
        var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiI2MTkyYTA4MDNiNGJlYjFlMmIzNTVmMTQiLCJleHAiOjE2MzkxMDc1MTcsImlzcyI6ImJlYWNvbnRyYWNrZXIuY29tLmJyIiwiYXVkIjoiYmVhY29udHJhY2tlci5jb20uYnIifQ.3pi7i5VisFBsmpjItd-ZUXxzt1QiskY-oN-djnXT4L4";
        var Auth = "Bearer " + token;
        
        client.BaseAddress = new Uri("https://nftbattleapi.beacontracker.software");
        client.DefaultRequestHeaders.Add("Authorization", Auth);
        
        var result = await client.GetAsync(parameters).ConfigureAwait(false);

        if (!result.IsSuccessStatusCode) throw new Exception(result.ReasonPhrase);
        var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

        var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var nfts = JsonSerializer.Deserialize<List<Nft>>(content, serializerOptions) ?? new List<Nft>();

        return nfts;
    }
}

//Chamada teste
var nfts = getUserNfts().Result;
foreach (var nft in nfts)
{
    Console.WriteLine(nft.Name);
}

//Classe Nft
public class Nft
{
    public string Id { get; set; }
    public string Token_id { get; set; }
    public string Image_url { get; set; }
    public string Name { get; set; }
    public int Attack { get; set; }
    public int Defence { get; set; }
    public int Health { get; set; }
}

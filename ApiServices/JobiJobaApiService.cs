using System.Net.Http.Headers;
using System.Text.Json;
using JobiJoba;

public class JobiJobaApiService{

    private readonly HttpClient _httpClient = new HttpClient(); 
    private readonly ConversionToAdService _conversionToAdService = new ConversionToAdService();
    private readonly TokenService _tokenService;

    private readonly string _client_Id;
    private readonly string _client_Secret;
    private readonly string _api_Search_Url;

    public JobiJobaApiService(HttpClient httpClient, ConversionToAdService conversionToAdService,TokenService tokenService, IConfiguration configuration){
        _httpClient = httpClient;
        _conversionToAdService = conversionToAdService;
        _tokenService = tokenService;
        _client_Id = configuration["ApiCredentials:ClientID"] ?? throw new InvalidOperationException("Le paramètre ClientID n'est pas configuré");
        _client_Secret = configuration["ApiCredentials:ClientSecret"] ?? throw new InvalidOperationException("Le paramètre ClientSecret n'est pas configuré");
        _api_Search_Url = configuration["ApiCredentials:ApiSearchUrl"] ?? throw new InvalidOperationException("Le paramètre ApiSearchUrl n'est pas configuré");
    }
    
    public async Task<List<Ad>> GetAdsFromApiAsync(){
        string token = await GetAccessToken();
        return await GetAds(token);
    }

    public async Task<string> GetAccessToken(){
        return await _tokenService.GetTokenAsync(_client_Id, _client_Secret);
    }

    public async Task<List<Ad>> GetAds(string accessToken){
        var queryParams = new Dictionary<string, string>();
        queryParams.Add("where", "Bordeaux");
        
        var uriBuilder = new UriBuilder(_api_Search_Url);
        var query = new FormUrlEncodedContent(queryParams).ReadAsStringAsync().Result;
        uriBuilder.Query = query;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri);

        if(!response.IsSuccessStatusCode){
            throw new HttpRequestException($"Erreur lors de la récupération des annonces : {response.StatusCode}");
        }

        var apiResponse = await response.Content.ReadAsStringAsync();
        var adsObject = JsonDocument.Parse(apiResponse);
        var ads = adsObject.RootElement.GetProperty("data").GetProperty("ads");
        
        List<Ad> AdsList = new List<Ad>();

        foreach (JsonElement ad in ads.EnumerateArray())
        {
            Ad adItem = _conversionToAdService.Convert(ad);
            AdsList.Add(adItem);
        }

        AdsList = AdsList.OrderByDescending(ad => ad.PublicationDate).ToList();

        return AdsList;
    }
}
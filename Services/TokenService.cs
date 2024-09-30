using System.Text.Json;
public class TokenService {
    private readonly HttpClient _httpClient;
    private string? _cachedToken;
    private DateTime _tokenExpirationTime;
    private readonly string _token_Url;

    public TokenService(HttpClient httpClient, IConfiguration configuration){
        _httpClient = httpClient;
        _cachedToken = null;
        _tokenExpirationTime = DateTime.MinValue;
        _token_Url = configuration["ApiCredentials:TokenUrl"] ?? throw new InvalidOperationException("Le paramètre TokenUrl n'est pas configuré");
    }

    public async Task<string> GetTokenAsync(string client_id, string client_secret){
        if(_cachedToken != null && _tokenExpirationTime > DateTime.UtcNow){
            return _cachedToken;
        }
        
        var jsonContent = new {
            client_id= client_id,
            client_secret = client_secret
        };
        var content = new StringContent(JsonSerializer.Serialize(jsonContent), System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(_token_Url, content);
        if(!response.IsSuccessStatusCode){
            throw new HttpRequestException($"Erreur lors de la récupération du token de connexion : {response.StatusCode}");
        }

        string jsonResponse = await response.Content.ReadAsStringAsync();

        var tokenObject = JsonDocument.Parse(jsonResponse);
        _cachedToken = tokenObject.RootElement.GetProperty("token").GetString();
        _tokenExpirationTime = DateTime.UtcNow.AddHours(1);

        return _cachedToken ?? "";
    }
}
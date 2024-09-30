using JobiJoba;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class JobiJobaController : ControllerBase {

    private readonly AdService _adService;

    public JobiJobaController(AdService adService){
         _adService = adService;
    }

    [HttpGet("GetAllAds")]
    public async Task<List<Ad>> GetAllAds()
    {
        return await _adService.GetOrLoadAdsAsync();
    }

    [HttpPost("RefreshAds")]
    public async Task<List<Ad>> RefreshAds(){
        return await _adService.RefreshAdsAsync();
    }

}
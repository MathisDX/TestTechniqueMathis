using JobiJoba;
using Microsoft.EntityFrameworkCore;

public class AdService{
    private readonly AppDbContext _context;
    private readonly JobiJobaApiService _jobijobaApiService;

    public AdService(AppDbContext context, JobiJobaApiService jobiJobaApiService){
        _context = context;
        _jobijobaApiService = jobiJobaApiService;
    }

    public async Task<List<Ad>> GetOrLoadAdsAsync(){
        
        var ads = await _context.Ads.ToListAsync();

        if(ads.Count == 0 ){
            ads = await _jobijobaApiService.GetAdsFromApiAsync();
            if(ads.Any()){
                await _context.Ads.AddRangeAsync(ads);
                await _context.SaveChangesAsync();
            }
        }
        
        return ads;
    }

    public async Task<List<Ad>> RefreshAdsAsync(){
        var ads = await _jobijobaApiService.GetAdsFromApiAsync();

        if(ads.Any()){
            _context.Ads.RemoveRange(_context.Ads);
            await _context.SaveChangesAsync();

            await _context.Ads.AddRangeAsync(ads);
            await _context.SaveChangesAsync();
        }
        
        return ads;
    }
}
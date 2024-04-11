using Microsoft.Extensions.Options;
using TutorLizard.BusinessLogic.Interfaces.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Options;

namespace TutorLizard.BusinessLogic.Data.Repositories.Json;

public class AdJsonRepository : JsonRepositoryBase<Ad>, IAdRepository 
{
    public AdJsonRepository(IOptions<DataJsonFilePaths> options) : base(options.Value.Ads)
    {
        
    }

    public Ad CreateAd(int tutorId,
          string subject,
          string title,
          string description,
          int category,
          double price,
          string location,
          bool isRemote)
    {
        Ad newAd = new Ad(GetNewAdID(), tutorId, subject, title, description, category, price, location, isRemote);
        Data.Add(newAd);

        SaveToJson();

        return newAd;
    }
    public List<Ad> GetAllAds()
    {
        return Data;
    }
    public Ad? GetAdById(int adId)
    {
        var ad = Data.FirstOrDefault(a => a.Id == adId);
        return ad;
    }
    public void UpdateAd(Ad ad)
    {
        var toUpdate = GetAdById(ad.Id);
        if (toUpdate is null)
            return;

        toUpdate.TutorId = ad.TutorId;
        toUpdate.Subject = ad.Subject;
        toUpdate.Title = ad.Title;
        toUpdate.Description = ad.Description;
        toUpdate.CategoryId = ad.CategoryId;
        toUpdate.Location = ad.Location;
        toUpdate.Price = ad.Price;
        toUpdate.IsRemote = ad.IsRemote;

        SaveToJson();
    }
    public void DeleteAdById(int adId)
    {
        var toDelete = GetAdById(adId);
        if (toDelete is null)
            return;

        Data.Remove(toDelete);

        SaveToJson();
    }

    private int GetNewAdID()
    {
        if (Data.Any() == true)
            return Data.Max(x => x.Id) + 1;
        else
            return 1;
    }
}

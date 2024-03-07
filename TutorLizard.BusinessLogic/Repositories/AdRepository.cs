using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorLizard.BusinessLogic.Interfaces.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Repositories
{
    public class AdRepository : IAdRepository
    {
        private readonly string _adFilePath = @"Data/ads.json";
        private List<Ad> _adList = new();

        public Ad CreateAd(int tutorId,
              string subject,
              string title,
              string description,
              string category,
              double price,
              string location,
              bool isRemote)
        {
            Ad newAd = new Ad(GetNewAdID(), tutorId, subject, title, description, category, price, location, isRemote);
            _adList.Add(newAd);
            SaveAdsToJson();

            return newAd;
        }
        public List<Ad> GetAllAds()
        {
            return _adList;
        }
        public Ad? GetAdById(int adId)
        {
            var ad = _adList.FirstOrDefault(a => a.Id == adId);
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
            toUpdate.Location = ad.Location;
            toUpdate.Price = ad.Price;
            toUpdate.IsRemote = ad.IsRemote;
            toUpdate.Category = ad.Category;

            SaveAdsToJson();
        }
        public void DeleteAdById(int adId)
        {
            var toDelete = GetAdById(adId);
            if (toDelete is null)
                return;

            _adList.Remove(toDelete);

            SaveAdsToJson();
        }
        public void LoadAdsFromJson()
        {
            //_adList = LoadFromJson<Ad>(_adFilePath);
        }
        public void SaveAdsToJson()
        {
            //SaveToJson(_adFilePath, _adList);
        }
        public int GetNewAdID()
        {
            if (_adList.Any() == true)
                return _adList.Max(x => x.Id) + 1;
            else
                return 1;
        }
    }
}

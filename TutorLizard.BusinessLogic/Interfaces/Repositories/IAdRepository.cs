using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Repositories
{
    public interface IAdRepository
    {
        public Ad CreateAd(int tutorId,
              string subject,
              string title,
              string description,
              string category,
              double price,
              string location,
              bool isRemote);
        public List<Ad> GetAllAds();
        public Ad? GetAdById(int adId);
        public void UpdateAd(Ad ad);
        public void DeleteAdById(int adId);
        public void LoadAdsFromJson();
        public void SaveAdsToJson();
        public int GetNewAdID();
    }
}

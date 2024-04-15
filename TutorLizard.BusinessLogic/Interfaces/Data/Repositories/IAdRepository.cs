using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Repositories
{
    public interface IAdRepository
    {
        public Ad CreateAd(int tutorId,
              string subject,
              string title,
              string description,
              int category,
              decimal price,
              string location,
              bool isRemote);
        public List<Ad> GetAllAds();
        public Ad? GetAdById(int adId);
        public void UpdateAd(Ad ad);
        public void DeleteAdById(int adId);
    }
}

using TutorLizard.BusinessLogic.Interfaces.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Repositories
{
    public class AdRequestRepository : IAdRequestRepository//, IRepositoryBase
    {
        private readonly string _adRequestFilePath = @"Data/ad_requests.json";
        private List<AdRequest> _adRequestList = new();
        public AdRequest CreateAdRequest(int adId,
                         int studentId,
                         string message,
                         bool isRemote)
        {
            AdRequest newAdRequest = new AdRequest(GetNewAdRequestID(),
                                               adId,
                                               studentId,
                                               message,
                                               isRemote,
                                               false);
            _adRequestList.Add(newAdRequest);
            SaveAdRequestsToJson();

            return newAdRequest;
        }
        public List<AdRequest> GetAllAdRequests()
        {
            return _adRequestList;
        }
        public AdRequest? GetAdRequestById(int adRequestId)
        {
            var adRequest = _adRequestList.FirstOrDefault(ar => ar.Id == adRequestId);
            return adRequest;
        }
        public void UpdateAdRequest(AdRequest adRequest)
        {
            var toUpdate = GetAdRequestById(adRequest.Id);
            if (toUpdate is null)
                return;

            toUpdate.AdId = adRequest.AdId;
            toUpdate.StudentId = adRequest.StudentId;
            toUpdate.IsAccepted = adRequest.IsAccepted;
            toUpdate.Message = adRequest.Message;
            toUpdate.IsRemote = adRequest.IsRemote;

            SaveAdRequestsToJson();
        }
        public void DeleteAdRequestById(int adRequestId)
        {
            var toDelete = GetAdRequestById(adRequestId);
            if (toDelete is null)
                return;

            _adRequestList.Remove(toDelete);

            SaveAdRequestsToJson();
        }
        public int GetNewAdRequestID()
        {
            if (_adRequestList.Any() == true)
                return _adRequestList.Max(x => x.Id) + 1;
            else
                return 1;
        }
        public void LoadAdRequestsFromJson()
        {
            //_adRequestList = LoadFromJson<AdRequest>(_adRequestFilePath);
        }
        public void SaveAdRequestsToJson()
        {
            //SaveToJson(_adRequestFilePath, _adRequestList);
        }
    }
}

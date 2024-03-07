using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Repositories
{
    public interface IAdRequestRepository
    {
        public AdRequest CreateAdRequest(int adId,
                                 int studentId,
                                 string message,
                                 bool isRemote);
        public List<AdRequest> GetAllAdRequests();
        public AdRequest? GetAdRequestById(int adRequestId);
        public void UpdateAdRequest(AdRequest adRequest);
        public void DeleteAdRequestById(int adRequestId);
        public void LoadAdRequestsFromJson();
        public void SaveAdRequestsToJson();
        public int GetNewAdRequestID();
    }
}

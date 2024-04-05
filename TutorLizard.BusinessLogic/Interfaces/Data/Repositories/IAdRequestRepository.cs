using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Data.Repositories;

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
 
}

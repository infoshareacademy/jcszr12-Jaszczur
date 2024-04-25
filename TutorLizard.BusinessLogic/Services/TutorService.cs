using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;
using TutorLizard.BusinessLogic.Interfaces.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;
public class TutorService : ITutorService
{
    // TODO inject needed repositories
    private readonly IAdRepository _adRepository;
    public TutorService(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }

    public async Task<IsUserTheAdOwnerResponse> IsUserTheAdOwner(IsUserTheAdOwnerRequest request)
    {
        // TODO add logic (this is only for tests)
        int adId = request.AdId;
        int userId = (int)request.UserId;

        Ad? ad = _adRepository.GetAdById(adId);
        bool isOwner = ad != null && ad.TutorId == userId;

        var response = new IsUserTheAdOwnerResponse
        {
            IsOwner = isOwner,
        };

        return response;
    }
}

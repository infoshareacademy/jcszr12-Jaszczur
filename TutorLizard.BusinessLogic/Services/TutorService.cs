using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Services;
public class TutorService : ITutorService
{
    public IDbRepository<AdRequest> _adRequestRepository;
    // TODO inject needed repositories
    public TutorService(IDbRepository<AdRequest> adRequestRepository)
    {
        _adRequestRepository = adRequestRepository;
    }

    public Task<IsUserTheAdOwnerResponse> IsUserTheAdOwner(IsUserTheAdOwnerRequest request)
    {
        // TODO add logic (this is only for tests)
        var response = new IsUserTheAdOwnerResponse
        {
            IsOwner = true
        };
        return Task.FromResult(response);
    }

    public async Task<UpdateTutorsPendingAdRequestResponse> UpdateAdRequest(UpdateTutorsPendingAdRequestRequest request)
    {
        var adRequest = await _adRequestRepository
            .Update(request.AdRequestId, entity =>
            {
                 entity.ReviewDate = DateTime.Now;
                 entity.ReplyMessage = request.ReplyMessage;
            });

        if (request.Action == UpdateTutorsPendingAdRequestRequest.UpdateAction.Accept)
        {
            adRequest = await _adRequestRepository
                .Update(request.AdRequestId, entity => entity.IsAccepted = true);
        }

        UpdateTutorsPendingAdRequestResponse response = new() 
        {
            UpdatedAdRequestDto =
            {
                Id = adRequest.Id,
                AdId = adRequest.AdId,
                StudentId = adRequest.StudentId,
                IsAccepted = adRequest.IsAccepted,
                Message = adRequest.Message,
                ReplyMessage = adRequest.ReplyMessage,
                ReviewDate = adRequest.ReviewDate,
                IsRemote = adRequest.IsRemote 
            }
        };
        
        return response;
    }
}

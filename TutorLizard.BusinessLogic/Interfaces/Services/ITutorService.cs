using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface ITutorService
{
    Task<AcceptScheduleItemRequestResponse> AcceptScheduleItemRequest(AcceptScheduleItemRequestRequest request);
    Task<TutorsScheduleForAdResponse> GetTutorsScheduleForAd(TutorsScheduleForAdRequest request);
    Task<IsUserTheAdOwnerResponse> IsUserTheAdOwner(IsUserTheAdOwnerRequest request);
    Task<UnacceptScheduleItemRequestResponse> UnacceptScheduleItemRequest(UnacceptScheduleItemRequestRequest request);
    Task<UpdateTutorsPendingAdRequestResponse> UpdateAdRequest(UpdateTutorsPendingAdRequestRequest request);
    Task<TutorsPendingAdRequestsResponse> ViewAllPendingAdRequests(TutorsPendingAdRequestsRequest request);
    Task<TutorAllAdRequestsResponse> ViewAllAdRequests(TutorAllAdRequestsRequest request);
    Task<TutorsAdsResponse> ViewTutorsAds(TutorsAdsRequest request);
}
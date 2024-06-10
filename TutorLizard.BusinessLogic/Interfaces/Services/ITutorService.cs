using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface ITutorService
{
    Task<AcceptScheduleItemRequestResponse> AcceptScheduleItemRequest(AcceptScheduleItemRequestRequest request);
    Task<CreateAdResponse> CrateAd(CreateAdRequest request);
    Task<GetTutorsScheduleForAdResponse> GetTutorsScheduleForAd(GetTutorsScheduleForAdRequest request);
    Task<CreateScheduleItemResponse> CreateScheduleItem(CreateScheduleItemRequest request);
    Task<IsUserTheAdOwnerResponse> IsUserTheAdOwner(IsUserTheAdOwnerRequest request);
    Task<UnacceptScheduleItemRequestResponse> UnacceptScheduleItemRequest(UnacceptScheduleItemRequestRequest request);
    Task<UpdateAdRequestResponse> UpdateAdRequest(UpdateAdRequestRequest request);
    Task<GetTutorsPendingAdRequestsResponse> GetTutorsPendingAdRequests(GetTutorsPendingAdRequestsRequest request);
    Task<GetTutorsAllAdRequestsResponse> GetTutorsAllAdRequests(GetTutorsAllAdRequestsRequest request);
    Task<GetTutorsAdsResponse> GetTutorsAds(GetTutorsAdsRequest request);
}